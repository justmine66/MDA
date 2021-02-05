using MDA.Domain.Events;
using MDA.Domain.Exceptions;
using MDA.Domain.Models;
using MDA.Domain.Notifications;
using MDA.Domain.Saga;
using MDA.Infrastructure.Async;
using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Handle(DomainCommandTransportMessage<TAggregateRootId> message) => HandleAsync(message).SyncRun();

        public async Task HandleAsync(DomainCommandTransportMessage<TAggregateRootId> message, CancellationToken token = default)
        {
            var command = message.DomainCommand;
            var commandId = command.Id;
            var commandType = command.GetType().FullName;
            var aggregateRootId = command.AggregateRootId;
            var aggregateRootType = command.AggregateRootType;
            var aggregateRootTypeFullName = aggregateRootType.FullName;
            var aggregateRootStringId = command.AggregateRootId.ToString();

            var aggregate = _cache.Get(aggregateRootStringId, aggregateRootType) ??
                            await _stateBackend.GetAsync(aggregateRootId, aggregateRootType, token);

            if (aggregate == null)
            {
                _logger.LogCritical($"Failed to restore aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootTypeFullName}] for domain command: [Id: {commandId}, Type: {commandType}] from cache and state backend.");

                return;
            }

            aggregate.MutatingDomainEvents?.Clear();
            aggregate.MutatingDomainNotifications?.Clear();

            try
            {
                var result = aggregate.HandleDomainCommand(command);
                if (!result.Succeed())
                {
                    _logger.LogError($"Handling domain command: [Id: {commandId}, Type: {commandType}] has a error: {result.Message}.");

                    return;
                }
            }
            catch (Exception e) when (e.InnerException is DomainException domainException)
            {
                await ProcessDomainExceptionAsync(command, domainException, token);

                return;
            }
            catch (Exception e)
            {
                await ProcessUnKnownExceptionAsync(command, e, token);

                return;
            }

            if (command.NeedReplyApplicationCommand())
            {
                await ReplyApplicationCommandAsync(command, token);
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

                await ProcessMutatingDomainNotificationsAsync(mutatingDomainNotifications, command, token);
            }

            if (!hasDomainEvent && !hasDomainNotification)
            {
                _logger.LogWarning($"The domain command: [Id: {commandId}, Type: {commandType}] apply neither domain event nor generate domain notification for aggregate root: [Id: {aggregateRootStringId}, Type: {aggregateRootTypeFullName}], please confirm whether the state will be lost.");
            }
        }

        #region [ private methods ]

        private async Task ProcessUnKnownExceptionAsync(IDomainCommand command, Exception exception, CancellationToken token)
        {
            var commandId = command.Id;
            var commandType = command.GetType().FullName;
            var canReturnOnDomainCommandHandled = command.ApplicationCommandReplyScheme == ApplicationCommandReplySchemes.OnDomainCommandHandled;

            if (!canReturnOnDomainCommandHandled)
            {
                _logger.LogError($"Handling domain command has a unknown exception, Id: {commandId}, Type: {commandType}, Exception: {LogFormatter.PrintException(exception)}.");

                return;
            }

            var domainExceptionMessage = new DomainExceptionMessage()
            {
                Message = exception.Message
            };

            domainExceptionMessage.FillFrom(command);

            await PublishDomainExceptionAsync(domainExceptionMessage, token);
        }

        private async Task ProcessDomainExceptionAsync(IDomainCommand command, DomainException domainException, CancellationToken token)
        {
            var commandId = command.Id;
            var commandType = command.GetType().FullName;
            var canReturnOnDomainCommandHandled = command.ApplicationCommandReplyScheme == ApplicationCommandReplySchemes.OnDomainCommandHandled;

            if (!canReturnOnDomainCommandHandled)
            {
                _logger.LogError($"Handling domain command has a domain exception, Id: {commandId}, Type: {commandType}, Exception: {LogFormatter.PrintException(domainException)}.");

                return;
            }

            var domainExceptionMessage = new DomainExceptionMessage()
            {
                Message = domainException.Message,
                Code = domainException.Code
            };

            domainExceptionMessage.FillFrom(command);

            await PublishDomainExceptionAsync(domainExceptionMessage, token);
        }

        private async Task ProcessMutatingDomainEventsAsync(IEnumerable<IDomainEvent> mutatingDomainEvents, IDomainCommand command, CancellationToken token)
        {
            mutatingDomainEvents.FillFrom(command);

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
                else
                {
                    var @event = mutatingDomainEvents.Single(it => it.Id == result.EventId);

                    await PublishDomainEventAsync(@event, token);
                }
            }
        }

        private async Task ProcessMutatingDomainNotificationsAsync(IEnumerable<IDomainNotification> mutatingDomainNotifications, IDomainCommand command, CancellationToken token)
        {
            foreach (var notification in mutatingDomainNotifications)
            {
                notification.FillFrom(command);

                await PublishDomainNotificationAsync(notification, token);

                if (notification.NeedReplyApplicationCommand(out var needRepliedNotification))
                {
                    await ReplyApplicationCommandAsync(needRepliedNotification, token);
                }
            }
        }

        private async Task ReplyApplicationCommandAsync(IDomainCommand command, CancellationToken token)
        {
            IDomainNotification notification;

            switch (command)
            {
                case IEndSubTransactionDomainCommand endCommand:
                    notification = new SagaTransactionDomainNotification(endCommand.Message, true);
                    break;
                default:
                    notification = new DomainCommandHandledNotification();
                    break;
            }

            notification.FillFrom(command);

            await PublishDomainNotificationAsync(notification, token);
        }

        private async Task ReplyApplicationCommandAsync(IEndSubTransactionDomainNotification notification, CancellationToken token)
        {
            var reply = new SagaTransactionDomainNotification(notification.Message);

            reply.FillFrom(notification);

            await PublishDomainNotificationAsync(reply, token);
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

        private async Task PublishDomainEventAsync(IDomainEvent @event, CancellationToken token)
        {
            var publisher = _serviceProvider.GetService<IDomainEventPublisher>();

            try
            {
                await publisher.PublishAsync(@event, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Publishing domain event has a exception: {LogFormatter.PrintException(e)}.");
            }
        }

        private async Task PublishDomainExceptionAsync(IDomainExceptionMessage exception, CancellationToken token)
        {
            var publisher = _serviceProvider.GetService<IDomainExceptionMessagePublisher>();

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