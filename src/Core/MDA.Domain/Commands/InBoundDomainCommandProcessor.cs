using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus;
using MDA.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Commands
{
    public class InBoundDomainCommandProcessor :
        IMessageHandler<DomainCommandTransportMessage>,
        IAsyncMessageHandler<DomainCommandTransportMessage>
    {
        private readonly IAggregateRootMemoryCache _cache;
        private readonly IAggregateRootStateBackend _stateBackend;
        private readonly ILogger _logger;

        public InBoundDomainCommandProcessor(
            IAggregateRootMemoryCache cache,
            IAggregateRootStateBackend stateBackend,
            ILogger<InBoundDomainCommandProcessor> logger)
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
            var commandId = command.Id;
            var commandType = command.GetType();
            var aggregateRootId = command.AggregateRootId;
            var aggregateRootType = command.AggregateRootType;

            var aggregate = _cache.Get(aggregateRootId, aggregateRootType) ??
                            await _stateBackend.GetAsync(aggregateRootId, aggregateRootType, token);

            if (aggregate == null)
            {
                _logger.LogCritical($"Failed to restore aggregate root: [Id: {aggregateRootId}, Type: {aggregateRootType.FullName}] for domain command: [Id: {commandId}, Type: {commandType.FullName}] from cache and state backend.");

                return;
            }

            try
            {
                var result = aggregate.HandleDomainCommand(command);
                if (!result.Succeed())
                {
                    _logger.LogError($"Handler domain command: [Id: {commandId}, Type: {commandType.FullName}] has a error: {result.Message}.");

                    return;
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Handler domain command: [Id: {commandId}, Type: {commandType.FullName}] has a unknown exception: {e}.");

                return;
            }

            var mutatingDomainEvents = aggregate.MutatingDomainEvents;
            if (mutatingDomainEvents.IsEmpty())
            {
                _logger.LogWarning($"The domain command: [Id: {commandId}, Type: {commandType.FullName}] dit not apply domain event for aggregate root: [Id: {aggregateRootId}, Type: {aggregateRootType.FullName}], please confirm whether the state will be lost.");

                return;
            }

            FillDomainCommandFor(mutatingDomainEvents, commandId, commandType);

            await _stateBackend.AppendMutatingDomainEventsAsync(mutatingDomainEvents, token);
        }

        private void FillDomainCommandFor(IEnumerable<IDomainEvent> domainEvents, string commandId, Type commandType)
        {
            foreach (var domainEvent in domainEvents)
            {
                domainEvent.DomainCommandId = commandId;
                domainEvent.DomainCommandType = commandType;
            }
        }
    }
}
