using MDA.Domain.Commands;
using MDA.MessageBus;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class AggregateRootMessageProcessor :
        IMessageHandler<DomainCommandTransportMessage>,
        IAsyncMessageHandler<DomainCommandTransportMessage>
    {
        private readonly IAggregateRootMemoryCache _cache;
        private readonly IAggregateRootStateBackend _stateBackend;
        private readonly ILogger _logger;

        public AggregateRootMessageProcessor(
            IAggregateRootMemoryCache cache,
            IAggregateRootStateBackend stateBackend,
            ILogger<AggregateRootMessageProcessor> logger)
        {
            _cache = cache;
            _stateBackend = stateBackend;
            _logger = logger;
        }

        public void Handle(DomainCommandTransportMessage message)
        {
            HandleAsync(message).GetAwaiter().GetResult();
        }

        public async Task HandleAsync(DomainCommandTransportMessage message, CancellationToken token = default)
        {
            var command = message.DomainCommand;
            var aggregateRootId = command.AggregateRootId;
            var aggregateRootType = command.AggregateRootType;

            var aggregate = _cache.Get(aggregateRootId, aggregateRootType) ??
                            await _stateBackend.GetAsync(aggregateRootId, aggregateRootType, token);

            if (aggregate == null)
            {
                _logger.LogCritical($"Failed to restore aggregate root state, DomainCommand: {command.GetType().FullName}, AggregateRootType: {aggregateRootType.FullName}, AggregateRootId: {aggregateRootId}.");

                return;
            }

            try
            {
                aggregate.OnDomainCommand(command);
            }
            catch (Exception e)
            {
                _logger.LogError($"Handler domain command has a error, reason: {e}.");

                return;
            }
        }
    }
}
