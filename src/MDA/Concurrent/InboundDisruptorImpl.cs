using Disruptor;
using Disruptor.Dsl;
using MDA.Cluster;
using MDA.Eventing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public class InboundDisruptorImpl<T> : IInboundDisruptor<T>
        where T : InboundEvent, new()
    {
        private readonly Disruptor<T> _disruptor;
        private readonly ILogger _logger;

        public InboundDisruptorImpl(
            IServiceProvider serviceProvider,
            IOptions<MdaOptions> options,
            ILogger<InboundDisruptorImpl<T>> logger)
        {
            _logger = logger;

            var ops = options.Value ?? MdaOptionsFactory.Create();
            var disOps = ops.DisruptorOptions;
            var settings = ops.ClusterSetting;

            _disruptor = new Disruptor<T>(() => new T(), disOps.InboundRingBufferSize, TaskScheduler.Current);

            var journaler = new InboundEventJournaler<T>();
            var businessProcessor = serviceProvider.GetRequiredService<IInBoundEventHandler<T>>();

            // Only the master node listens directly to input events and runs a replicator.
            if (settings.AppMode.Environment.IsMaster)
            {
                // The replicator broadcasts the input events to the slave nodes.
                // Should the master node go down, it's lack of heartbeat will be noticed, another node becomes master, starts processing input events, and starts its replicator.
                var replicator = new InboundEventReplicator<T>();
                _disruptor.HandleEventsWith(journaler, replicator).Then(businessProcessor);
            }
            else
                _disruptor.HandleEventsWith(journaler).Then(businessProcessor);

            _disruptor.Start();
        }

        public Task<bool> PublishInboundEventAsync<TMessage>(IEventTranslatorTwoArg<T, string, TMessage> translator, string messageKey, TMessage message)
        {
            try
            {
                _logger.LogInformation($"Prepare to publish inbound event[id: {messageKey},message: {message.ToString()}]");

                _disruptor.PublishEvent(translator, messageKey, message);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Failed publish inbound event[id: {messageKey},message: {message.ToString()}], ex: {e}");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
