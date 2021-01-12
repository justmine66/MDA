using MDA.Domain.Events;
using MDA.Domain.Exceptions;
using MDA.Domain.Models;
using MDA.Domain.Notifications;
using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Commands
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class InBoundDomainCommandProcessor<TAggregateRootId> :
        IMessageHandler<DomainCommandTransportMessage<TAggregateRootId>>,
        IAsyncMessageHandler<DomainCommandTransportMessage<TAggregateRootId>>
    {
        private readonly ILogger _logger;
        private readonly IAggregateRootMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAggregateRootStateBackend _stateBackend;

        public InBoundDomainCommandProcessor(
            IAggregateRootMemoryCache cache,
            IServiceProvider serviceProvider,
            IAggregateRootStateBackend stateBackend,
            ILogger<InBoundDomainCommandProcessor<TAggregateRootId>> logger)
        {
            _cache = cache;
            _logger = logger;
            _stateBackend = stateBackend;
            _serviceProvider = serviceProvider;
        }

        public void Handle(DomainCommandTransportMessage<TAggregateRootId> message) => HandleAsync(message).GetAwaiter().GetResult();

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
                _logger.LogCritical($"Failed to restore aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootType.FullName}] for domain command: [Id: {commandId}, Type: {commandType.FullName}] from cache and state backend.");

                return;
            }

            aggregate.MutatingDomainEvents?.Clear();
            aggregate.MutatingDomainNotifications?.Clear();

            try
            {
                var result = aggregate.HandleDomainCommand(command);
                if (!result.Succeed())
                {
                    _logger.LogError($"Handler domain command: [Id: {commandId}, Type: {commandType.FullName}] has a error: {result.Message}.");

                    return;
                }
            }
            catch (Exception e) when (e.InnerException is DomainException domainException)
            {
                var domainExceptionMessage = new DomainExceptionMessage()
                {
                    Message = domainException.Message,
                    Code = (int)DomainStatusCodes.DomainCommandHandled
                };

                domainExceptionMessage.FillDomainCommandInfo(command);

                await PublishDomainExceptionAsync(domainExceptionMessage, token);

                return;
            }
            catch (Exception e)
            {
                _logger.LogError($"Handler domain command has a unknown exception, Id: {commandId}, Type: {commandType.FullName}, Exception: {e}.");

                return;
            }

            var hasDomainEvent = false;
            var mutatingDomainEvents = aggregate.MutatingDomainEvents;
            if (mutatingDomainEvents.IsNotEmpty())
            {
                hasDomainEvent = true;

                await ProcessMutatingDomainEventsAsync(aggregate.MutatingDomainEvents, command, token);
            }

            var hasDomainNotification = false;
            var mutatingDomainNotifications = aggregate.MutatingDomainNotifications;
            if (mutatingDomainNotifications.IsNotEmpty())
            {
                hasDomainNotification = true;

                await PublishMutatingDomainNotificationsAsync(mutatingDomainNotifications, command, token);
            }

            if (!hasDomainEvent && !hasDomainNotification)
            {
                _logger.LogWarning($"The domain command: [Id: {commandId}, Type: {commandType.FullName}] apply neither domain event nor generate domain notification for aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootType.FullName}], please confirm whether the state will be lost.");
            }
        }

        #region [ private methods ]

        private async Task ProcessMutatingDomainEventsAsync(
            IEnumerable<IDomainEvent> mutatingDomainEvents,
            IDomainCommand command,
            CancellationToken token = default)
        {
            mutatingDomainEvents.FillDomainCommandInfo(command);

            IEnumerable<DomainEventResult> appendResults;
            try
            {
                appendResults = await _stateBackend.AppendMutatingDomainEventsAsync(mutatingDomainEvents, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Append domain event to state backend has a exception: {LogFormatter.PrintException(e)}.");

                return;
            }

            if (appendResults.IsEmpty()) return;

            foreach (var result in appendResults)
            {
                if (!(result.StorageSucceed() ||
                      result.HandleSucceed()))
                {
                    _logger.LogError($"Append domain event has a error: {result.Message}.");
                }
            }
        }

        private async Task PublishMutatingDomainNotificationsAsync(
            IEnumerable<IDomainNotification> mutatingDomainNotifications,
            IDomainCommand command,
            CancellationToken token = default)
        {
            mutatingDomainNotifications.FillDomainCommandInfo(command);

            foreach (var notification in mutatingDomainNotifications)
            {
                await PublishDomainNotificationAsync(notification, token);
            }
        }

        private async Task PublishDomainNotificationAsync(IDomainNotification notification, CancellationToken token)
        {
            var publisher = _serviceProvider.GetService<IDomainNotificationPublisher>();

            try
            {
                await publisher.PublishAsync(notification, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Publishing domain notification has a exception: {LogFormatter.PrintException(e)}.");
            }
        }

        private async Task PublishDomainExceptionAsync(IDomainExceptionMessage exception, CancellationToken token)
        {
            var publisher = _serviceProvider.GetService<IDomainExceptionPublisher>();

            try
            {
                await publisher.PublishAsync(exception, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Publishing domain exception has a exception: {LogFormatter.PrintException(e)}.");
            }
        }

        #endregion
    }
}