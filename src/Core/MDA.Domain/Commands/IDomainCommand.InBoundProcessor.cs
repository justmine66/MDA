using MDA.Domain.Events;
using MDA.Domain.Models;
using MDA.MessageBus;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MDA.MessageBus.DependencyInjection;

namespace MDA.Domain.Commands
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class InBoundDomainCommandProcessor<TAggregateRootId> :
        IMessageHandler<DomainCommandTransportMessage<TAggregateRootId>>,
        IAsyncMessageHandler<DomainCommandTransportMessage<TAggregateRootId>>
    {
        private readonly IAggregateRootStateBackend _stateBackend;
        private readonly IAggregateRootMemoryCache _cache;
        private readonly ILogger _logger;

        public InBoundDomainCommandProcessor(
            IAggregateRootMemoryCache cache,
            IAggregateRootStateBackend stateBackend,
            ILogger<InBoundDomainCommandProcessor<TAggregateRootId>> logger)
        {
            _cache = cache;
            _stateBackend = stateBackend;
            _logger = logger;
        }

        public void Handle(DomainCommandTransportMessage<TAggregateRootId> message)
        {
            HandleAsync(message).GetAwaiter().GetResult();
        }

        public async Task HandleAsync(DomainCommandTransportMessage<TAggregateRootId> message, CancellationToken token = default)
        {
            var command = message.DomainCommand;
            var commandId = command.Id;
            var commandType = command.GetType();
            var aggregateRootId = command.AggregateRootId;
            var aggregateRootStringId = command.AggregateRootId.ToString();
            var aggregateRootType = command.AggregateRootType;

            var aggregate = _cache.Get(aggregateRootStringId, aggregateRootType) ??
                            await _stateBackend.GetAsync(aggregateRootId, aggregateRootType, token);

            if (aggregate == null)
            {
                _logger.LogCritical(
                    $"Failed to restore aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootType.FullName}] for domain command: [Id: {commandId}, Type: {commandType.FullName}] from cache and state backend.");

                return;
            }

            aggregate.MutatingDomainEvents?.Clear();

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
                _logger.LogWarning($"The domain command: [Id: {commandId}, Type: {commandType.FullName}] dit not apply domain event for aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootType.FullName}], please confirm whether the state will be lost.");

                return;
            }

            mutatingDomainEvents.FillDomainCommandInfo(command);

            var results = await _stateBackend.AppendMutatingDomainEventsAsync(mutatingDomainEvents, token);
            if (results.IsEmpty()) return;

            foreach (var result in results)
            {
                if (!(result.StorageSucceed() ||
                    result.HandleSucceed()))
                {
                    _logger.LogError($"Append domain event has a error: {result.Message}.");
                }
            }
        }
    }
}
