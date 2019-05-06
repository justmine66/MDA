using System;
using System.Threading.Tasks;
using Disruptor;
using Disruptor.Dsl;
using MDA.Cluster;
using MDA.Commanding;
using MDA.Eventing;
using MDA.EventSourcing;
using MDA.EventStoring;
using MDA.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MDA.Concurrent
{
    public class InboundDisruptorImpl<TDomainEvent> : IInboundDisruptor<TDomainEvent>
        where TDomainEvent : DomainEvent, new()
    {
        private readonly Disruptor<TDomainEvent> _disruptor;
        private readonly ILogger _logger;

        public InboundDisruptorImpl(
            ILogger<InboundDisruptorImpl<TDomainEvent>> logger,
            IServiceProvider serviceProvider,
            IOptions<MdaOptions> options,
            IEventStore eventStore)
        {
            _logger = logger;

            var ops = options.Value;
            var disOps = ops.DisruptorOptions;
            var settings = ops.ClusterSetting;

            _disruptor = new Disruptor<TDomainEvent>(() => new TDomainEvent(), disOps.InboundRingBufferSize, TaskScheduler.Current);

            var journaler = new InboundEventJournaler<TDomainEvent>(eventStore);
            var businessProcessor = serviceProvider.GetRequiredService<IInBoundDomainEventHandler<TDomainEvent>>();

            // Only the master node listens directly to input events and runs a replicator.
            if (settings.AppMode.Environment.IsMaster)
            {
                // The replicator broadcasts the input events to the slave nodes.
                // Should the master node go down, it's lack of heartbeat will be noticed, another node becomes master, starts processing input events, and starts its replicator.
                var replicator = new InboundEventReplicator<TDomainEvent>();
                _disruptor.HandleEventsWith(journaler, replicator).Then(businessProcessor);
            }
            else
                _disruptor.HandleEventsWith(journaler).Then(businessProcessor);

            _disruptor.Start();
        }

        public Task<bool> PublishInboundEventAsync<TTranslator, TCommand>(string principal, TCommand command)
            where TCommand : ICommand
            where TTranslator : IEventTranslatorTwoArg<TDomainEvent, string, TCommand>, new()
        {
            try
            {
                _logger.LogInformation($"Prepare to publish inbound domain event[principal: {principal}, command: {command}]");

                var translator = new TTranslator();
                _disruptor.PublishEvent(translator, principal, command);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Failed publish inbound domain event[principal: {principal}, command: {command}], ex: {e}");
                return Task.FromResult(false);
            }
        }
    }
}
