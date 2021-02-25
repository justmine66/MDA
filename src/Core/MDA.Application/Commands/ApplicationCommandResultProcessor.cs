using MDA.Domain.Shared;
using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Events;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;
using MDA.Domain.Shared.Saga;
using MDA.Infrastructure.Scheduling;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

namespace MDA.Application.Commands
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class ApplicationCommandResultProcessor : IApplicationCommandResultListener,
        IMessageHandler<DomainExceptionMessage>,
        IMessageHandler<SagaTransactionDomainNotification>,
        IMessageHandler<DomainCommandHandledNotification>,
        IMessageHandler<DomainEventHandledNotification>
    {
        private static readonly ConcurrentDictionary<string, ApplicationCommandExecutionPromise> ExecutingPromiseDict = new ConcurrentDictionary<string, ApplicationCommandExecutionPromise>();

        private readonly ITimer _timer;
        private readonly ApplicationCommandOptions _commandOptions;
        private readonly ILogger<ApplicationCommandResultProcessor> _logger;

        public ApplicationCommandResultProcessor(
            ILogger<ApplicationCommandResultProcessor> logger,
            IOptions<ApplicationCommandOptions> executeOptions,
            ITimer timer)
        {
            _timer = timer;
            _logger = logger;
            _commandOptions = executeOptions.Value;
        }

        public void AddExecutingPromise(ApplicationCommandExecutionPromise promise)
        {
            var command = promise.ApplicationCommand;

            if (ExecutingPromiseDict.TryAdd(command.Id, promise))
            {
                var delay = _commandOptions.ExecutionOptions.TimeOutInSeconds;

                _timer.NewTimeout(new ActionTimerTask(it => CancelApplicationCommand(promise, delay)), TimeSpan.FromSeconds(delay));
            }
            else
            {
                _logger.LogWarning($"The application command registered，Id:{command.Id}, Type:{command.GetType().FullName}, ReplyScheme:{command.ReplyScheme}.");
            }
        }

        public void AddExecutingPromise<TPayload>(ApplicationCommandExecutionPromise<TPayload> promise)
        {
            var command = promise.ApplicationCommand;

            if (ExecutingPromiseDict.TryAdd(command.Id, promise))
            {
                var delay = _commandOptions.ExecutionOptions.TimeOutInSeconds;

                _timer.NewTimeout(new ActionTimerTask(it => CancelApplicationCommand(promise, delay)), TimeSpan.FromSeconds(delay));
            }
            else
            {
                _logger.LogWarning($"The application command registered，Id:{command.Id}, Type:{command.GetType().FullName}, ReplyScheme:{command.ReplyScheme}.");
            }
        }

        public void Handle(DomainExceptionMessage exception) => SetApplicationCommandResult(exception);

        public void Handle(SagaTransactionDomainNotification notification) => SetApplicationCommandResult(notification);

        public void Handle(DomainCommandHandledNotification notification) => SetApplicationCommandResult(notification);

        public void Handle(DomainEventHandledNotification notification) => SetApplicationCommandResult(notification);

        #region [ private methods ]

        private void SetApplicationCommandResult(DomainExceptionMessage exception)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;

            const string prefix = "Received the execution notification of application Command";

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(exception, prefix, "but task source does not exist"));

                return;
            }

            var replyScheme = executionPromise.ApplicationCommand.ReplyScheme;

            switch (replyScheme)
            {
                case ApplicationCommandReplySchemes.None:
                    _logger.LogWarning(FormatReplyMessage(exception, prefix, $"but the return schema is {replyScheme}"));
                    break;
                case ApplicationCommandReplySchemes.OnDomainCommandHandled:

                    var replySchemeReceived = exception.ApplicationCommandReplyScheme;

                    if (replyScheme != replySchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(exception, prefix, $"but return schema mis match, expected: {replyScheme}, actual: {replySchemeReceived}"));
                    }

                    var result = ApplicationCommandResult.Failed(applicationCommandId, applicationCommandType, exception.Message);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(exception, "Failed to set the domain exception of application Command", $"ReplyScheme:{replyScheme}, result:{result}"));
                    }

                    break;
                case ApplicationCommandReplySchemes.OnDomainEventHandled:
                    _logger.LogWarning(FormatReplyMessage(exception, prefix, $"but the return schema:{replyScheme} mis-match"));
                    break;
                default:
                    _logger.LogWarning(FormatReplyMessage(exception, prefix, $"but the return schema:{replyScheme} not supported"));
                    break;
            }
        }

        private void SetApplicationCommandResult(SagaTransactionDomainNotification notification)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;

            const string prefix = "Received the saga domain notification of application Command";

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(notification, prefix, "but task source does not exist"));

                return;
            }

            var replyScheme = executionPromise.ApplicationCommand.ReplyScheme;

            switch (replyScheme)
            {
                case ApplicationCommandReplySchemes.None:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema is {replyScheme}"));
                    break;

                case ApplicationCommandReplySchemes.OnDomainCommandHandled:

                    var replySchemeReceived = notification.ApplicationCommandReplyScheme;

                    if (replyScheme != replySchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but return schema mis match, expected: {replyScheme}, actual: {replySchemeReceived}"));
                    }

                    var status = notification.IsCompleted
                        ? ApplicationCommandStatus.Succeed
                        : ApplicationCommandStatus.Failed;
                    var result = new ApplicationCommandResult<string>(applicationCommandId, applicationCommandType, status, notification.Message);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(notification, "Failed to set the saga domain notification of application Command", $"ReplyScheme:{replyScheme}, message:{notification.Message}"));
                    }

                    break;

                case ApplicationCommandReplySchemes.OnDomainEventHandled:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} mis-match"));
                    break;

                default:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} not supported"));
                    break;
            }
        }

        private void SetApplicationCommandResult(DomainCommandHandledNotification notification)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;

            const string prefix = "Received the domain command handled notification of application Command";

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(notification, prefix, "but task source does not exist"));

                return;
            }

            var replyScheme = executionPromise.ApplicationCommand.ReplyScheme;

            switch (replyScheme)
            {
                case ApplicationCommandReplySchemes.None:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema is {replyScheme}"));
                    break;

                case ApplicationCommandReplySchemes.OnDomainCommandHandled:

                    var replySchemeReceived = notification.ApplicationCommandReplyScheme;

                    if (replyScheme != replySchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but return schema mis match, expected: {replyScheme}, actual: {replySchemeReceived}"));
                    }

                    var result = ApplicationCommandResult.Succeed(applicationCommandId, applicationCommandType);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(notification, "Failed to set the domain command handled notification of application Command", $"ReplyScheme:{replyScheme}"));
                    }

                    break;

                case ApplicationCommandReplySchemes.OnDomainEventHandled:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} mis-match"));
                    break;

                default:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} not supported"));
                    break;
            }
        }

        private void SetApplicationCommandResult(DomainEventHandledNotification notification)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;

            const string prefix = "Received the domain event handled notification of application Command";

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(notification, prefix, "but task source does not exist"));

                return;
            }

            var replyScheme = executionPromise.ApplicationCommand.ReplyScheme;

            switch (replyScheme)
            {
                case ApplicationCommandReplySchemes.None:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema is {replyScheme}"));
                    break;

                case ApplicationCommandReplySchemes.OnDomainCommandHandled:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} mis-match"));
                    break;

                case ApplicationCommandReplySchemes.OnDomainEventHandled:
                    var replySchemeReceived = notification.ApplicationCommandReplyScheme;

                    if (replyScheme != replySchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but return schema mis match, expected: {replyScheme}, actual: {replySchemeReceived}"));
                    }

                    var result = ApplicationCommandResult.Succeed(applicationCommandId, applicationCommandType);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(notification, "Failed to set the domain command handled notification of application Command", $"ReplyScheme:{replyScheme}"));
                    }
                    break;

                default:
                    _logger.LogWarning(FormatReplyMessage(notification, prefix, $"but the return schema:{replyScheme} not supported"));
                    break;
            }
        }

        private void CancelApplicationCommand(ApplicationCommandExecutionPromise promise, int delay)
        {
            var command = promise.ApplicationCommand;
            var commandId = command.Id;
            var commandType = command.GetType().FullName;

            if (!ExecutingPromiseDict.TryRemove(commandId, out var executionPromise))
            {
                _logger.LogTrace($"Over execution timeout upper limit: {delay} seconds, Cancelling application Command, Id:{commandId}, Type:{commandType}, but task source does not exist.");

                return;
            }

            var replyScheme = command.ReplyScheme;
            var result = ApplicationCommandResult.TimeOuted(commandId, commandType, "The execution of application command timeout, please try again.");

            _logger.LogWarning(executionPromise.TrySetResult(result)
                ? $"Over execution timeout upper limit: {delay} seconds, cancelled application Command, Id:{commandId}, Type:{commandType}, ReplyScheme:{replyScheme}."
                : $"Over execution timeout upper limit: {delay} seconds, Failed to cancel application Command, Id:{commandId}, Type:{commandType}, ReplyScheme:{replyScheme}.");
        }

        private string FormatReplyMessage(DomainExceptionMessage exception, string prefix, string reason)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;
            var applicationCommandReplyScheme = exception.ApplicationCommandReplyScheme;
            var domainCommandId = exception.DomainCommandId;
            var domainCommandType = exception.DomainCommandType;
            var aggregateRootId = exception.AggregateRootId;
            var aggregateRootType = exception.AggregateRootType;

            return $"{prefix}[Id:{applicationCommandId}, Type:{applicationCommandType}, ReplyScheme: {applicationCommandReplyScheme}], domain command[Id:{domainCommandId}, Type:{domainCommandType}], aggregate root[Id:{aggregateRootId}, Type:{aggregateRootType}], {reason}.";
        }

        private string FormatReplyMessage(DomainNotification notification, string prefix, string postfix)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;
            var domainCommandId = notification.DomainCommandId;
            var domainCommandType = notification.DomainCommandType;
            var aggregateRootId = notification.AggregateRootId;
            var aggregateRootType = notification.AggregateRootType;

            return $"{prefix}[Id:{applicationCommandId}, Type:{applicationCommandType}], domain command[Id:{domainCommandId}, Type:{domainCommandType}], aggregate root[Id:{aggregateRootId}, Type:{aggregateRootType}], {postfix}.";
        }

        #endregion
    }
}