using MDA.Domain;
using MDA.Domain.Exceptions;
using MDA.Domain.Saga;
using MDA.Infrastructure.Scheduling;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class ApplicationCommandResultProcessor : IApplicationCommandResultListener,
        IMessageHandler<DomainExceptionMessage>,
        IAsyncMessageHandler<DomainExceptionMessage>,
        IMessageHandler<SagaTransactionDomainNotification>,
        IAsyncMessageHandler<SagaTransactionDomainNotification>
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
                _logger.LogWarning($"The application command registered，Id:{command.Id}, Type:{command.GetType().FullName}, ReturnScheme:{command.ReturnScheme}.");
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
                _logger.LogWarning($"The application command registered，Id:{command.Id}, Type:{command.GetType().FullName}, ReturnScheme:{command.ReturnScheme}.");
            }
        }

        public void Handle(DomainExceptionMessage exception) => SetApplicationCommandResult(exception);

        public async Task HandleAsync(DomainExceptionMessage exception, CancellationToken token = default)
        {
            SetApplicationCommandResult(exception);

            await Task.CompletedTask;
        }

        public void Handle(SagaTransactionDomainNotification notification) => SetApplicationCommandResult(notification);

        public async Task HandleAsync(SagaTransactionDomainNotification notification, CancellationToken token = default)
        {
            SetApplicationCommandResult(notification);

            await Task.CompletedTask;
        }

        #region [ private methods ]

        private void SetApplicationCommandResult(DomainExceptionMessage exception)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(exception, "but task source does not exist"));

                return;
            }

            var returnScheme = executionPromise.ApplicationCommand.ReturnScheme;

            switch (returnScheme)
            {
                case ApplicationCommandResultReturnSchemes.None:
                    _logger.LogWarning(FormatReplyMessage(exception, $"but the return schema is {returnScheme}"));
                    break;
                case ApplicationCommandResultReturnSchemes.OnDomainCommandHandled:

                    var returnSchemeReceived = exception.ApplicationCommandReturnScheme;

                    if (returnScheme != returnSchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(exception, $"but return schema mis match, expected: {returnScheme}, actual: {returnSchemeReceived}"));
                    }

                    var result = ApplicationCommandResult.Failed(applicationCommandId, applicationCommandType, exception.Message);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(exception, "Failed to set the execution exception of application Command", $"ReturnScheme:{returnScheme}, result:{result}"));
                    }

                    break;
                case ApplicationCommandResultReturnSchemes.OnDomainEventHandled:
                    _logger.LogWarning(FormatReplyMessage(exception, $"but the return schema:{returnScheme} not supported"));
                    break;
                default:
                    _logger.LogWarning(FormatReplyMessage(exception, $"but the return schema:{returnScheme} not supported"));
                    break;
            }
        }

        private void SetApplicationCommandResult(SagaTransactionDomainNotification notification)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;

            if (!ExecutingPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning(FormatReplyMessage(notification, "but task source does not exist"));

                return;
            }

            var returnScheme = executionPromise.ApplicationCommand.ReturnScheme;

            switch (returnScheme)
            {
                case ApplicationCommandResultReturnSchemes.None:
                    _logger.LogWarning(FormatReplyMessage(notification, $"but the return schema is {returnScheme}"));
                    break;

                case ApplicationCommandResultReturnSchemes.OnDomainCommandHandled:

                    var returnSchemeReceived = notification.ApplicationCommandReturnScheme;

                    if (returnScheme != returnSchemeReceived)
                    {
                        _logger.LogWarning(FormatReplyMessage(notification, $"but return schema mis match, expected: {returnScheme}, actual: {returnSchemeReceived}"));
                    }

                    var status = notification.IsCompleted
                        ? ApplicationCommandStatus.Succeed
                        : ApplicationCommandStatus.Failed;

                    var result = new ApplicationCommandResult<string>(applicationCommandId, applicationCommandType, status, notification.Message);

                    if (!executionPromise.TrySetResult(result))
                    {
                        _logger.LogError(FormatReplyMessage(notification, "Failed to set the execution notification of application Command", $"ReturnScheme:{returnScheme}, message:{notification.Message}"));
                    }

                    break;

                case ApplicationCommandResultReturnSchemes.OnDomainEventHandled:
                    _logger.LogWarning(FormatReplyMessage(notification, $"but the return schema:{returnScheme} not supported"));
                    break;

                default:
                    _logger.LogWarning(FormatReplyMessage(notification, $"but the return schema:{returnScheme} not supported"));
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

            var returnScheme = command.ReturnScheme;
            var result = ApplicationCommandResult.TimeOuted(commandId, commandType, "The execution of application command timeout, please try again.");

            _logger.LogWarning(executionPromise.TrySetResult(result)
                ? $"Over execution timeout upper limit: {delay} seconds, cancelled application Command, Id:{commandId}, Type:{commandType}, ReturnScheme:{returnScheme}."
                : $"Over execution timeout upper limit: {delay} seconds, Failed to cancel application Command, Id:{commandId}, Type:{commandType}, ReturnScheme:{returnScheme}.");
        }

        private string FormatReplyMessage(DomainExceptionMessage exception, string reason)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;
            var domainCommandId = exception.DomainCommandId;
            var domainCommandType = exception.DomainCommandType;
            var aggregateRootId = exception.AggregateRootId;
            var aggregateRootType = exception.AggregateRootType;

            return $"Received the execution exception of application Command[Id:{applicationCommandId}, Type:{applicationCommandType}], domain command[Id:{domainCommandId}, Type:{domainCommandType}], aggregate root[Id:{aggregateRootId}, Type:{aggregateRootType}], {reason}.";
        }

        private string FormatReplyMessage(DomainExceptionMessage exception, string prefix, string postfix)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;
            var domainCommandId = exception.DomainCommandId;
            var domainCommandType = exception.DomainCommandType;
            var aggregateRootId = exception.AggregateRootId;
            var aggregateRootType = exception.AggregateRootType;

            return $"{prefix}[Id:{applicationCommandId}, Type:{applicationCommandType}], domain command[Id:{domainCommandId}, Type:{domainCommandType}], aggregate root[Id:{aggregateRootId}, Type:{aggregateRootType}], {postfix}.";
        }

        private string FormatReplyMessage(SagaTransactionDomainNotification notification, string reason)
        {
            var applicationCommandId = notification.ApplicationCommandId;
            var applicationCommandType = notification.ApplicationCommandType;
            var domainCommandId = notification.DomainCommandId;
            var domainCommandType = notification.DomainCommandType;
            var aggregateRootId = notification.AggregateRootId;
            var aggregateRootType = notification.AggregateRootType;

            return $"Received the execution notification of application Command[Id:{applicationCommandId}, Type:{applicationCommandType}], domain command[Id:{domainCommandId}, Type:{domainCommandType}], aggregate root[Id:{aggregateRootId}, Type:{aggregateRootType}], {reason}.";
        }

        private string FormatReplyMessage(SagaTransactionDomainNotification notification, string prefix, string postfix)
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