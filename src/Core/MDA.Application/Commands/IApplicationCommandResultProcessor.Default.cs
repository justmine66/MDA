using MDA.Domain;
using MDA.Domain.Exceptions;
using MDA.Infrastructure.Scheduling;
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
    public class DefaultApplicationResultProcessor : IApplicationCommandResultProcessor,
        IDomainExceptionHandler<DomainExceptionMessage>,
        IAsyncDomainExceptionHandler<DomainExceptionMessage>
    {
        private static readonly ConcurrentDictionary<string, ApplicationCommandExecutionPromise> ExecutionPromiseDict = new ConcurrentDictionary<string, ApplicationCommandExecutionPromise>();

        private readonly ITimer _timer;
        private readonly ApplicationCommandOptions _commandOptions;
        private readonly ILogger<DefaultApplicationResultProcessor> _logger;

        public DefaultApplicationResultProcessor(
            ILogger<DefaultApplicationResultProcessor> logger,
            IOptions<ApplicationCommandOptions> executeOptions,
            ITimer timer)
        {
            _timer = timer;
            _logger = logger;
            _commandOptions = executeOptions.Value;
        }

        public void AddExecutionPromise(ApplicationCommandExecutionPromise promise)
        {
            var command = promise.ApplicationCommand;

            if (ExecutionPromiseDict.TryAdd(command.Id, promise))
            {
                var delay = _commandOptions.ExecutionOptions.TimeOutInSeconds;

                _timer.NewTimeout(new ActionTimerTask(it => CancelApplicationCommand(promise, delay)), TimeSpan.FromSeconds(delay));
            }
            else
            {
                _logger.LogWarning($"The application command registered，Id:{command.Id}, Type:{command.GetType().FullName}, ReturnScheme:{command.ReturnScheme}.");
            }
        }

        public void AddExecutionPromise<TPayload>(ApplicationCommandExecutionPromise<TPayload> promise)
        {
            var command = promise.ApplicationCommand;

            if (ExecutionPromiseDict.TryAdd(command.Id, promise))
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

        private void SetApplicationCommandResult(DomainExceptionMessage exception)
        {
            var applicationCommandId = exception.ApplicationCommandId;
            var applicationCommandType = exception.ApplicationCommandType;

            if (!ExecutionPromiseDict.TryRemove(applicationCommandId, out var executionPromise))
            {
                _logger.LogWarning($"Received the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, but task source does not exist.");

                return;
            }

            var returnScheme = executionPromise.ApplicationCommand.ReturnScheme;

            switch (returnScheme)
            {
                case ApplicationCommandResultReturnSchemes.None:
                    _logger.LogWarning($"Received the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, but the return schema is {returnScheme}.");
                    break;
                case ApplicationCommandResultReturnSchemes.OnDomainCommandHandled:
                    // 只处理系统内置业务状态码，不处理用户自定义业务状态码。
                    if (Enum.TryParse<DomainStatusCodes>(exception.Code.ToString(), out var domainStatusCode))
                    {
                        ApplicationCommandStatus status;

                        switch (domainStatusCode)
                        {
                            case DomainStatusCodes.DomainCommandHandled:
                                status = ApplicationCommandStatus.Failed;
                                break;
                            default:
                                _logger.LogError($"Received the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, but return schema mis match, expected:{returnScheme}, actual:{domainStatusCode}.");
                                return;
                        }

                        var result = new ApplicationCommandResult<string>(applicationCommandId, applicationCommandType, status, exception.Message);

                        if (!executionPromise.TrySetResult(result))
                        {
                            _logger.LogError($"Failed to set the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, ReturnScheme:{returnScheme}, result:{result}.");
                        }
                    }
                    break;
                case ApplicationCommandResultReturnSchemes.OnDomainEventHandled:
                    _logger.LogWarning($"Received the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, but the return schema:{returnScheme} not supported.");
                    break;
                default:
                    _logger.LogError($"Received the execution result of application Command, Id:{applicationCommandId}, Type:{applicationCommandType}, but the return schema:{returnScheme} not supported.");
                    break;
            }
        }

        private void CancelApplicationCommand(ApplicationCommandExecutionPromise promise, int delay)
        {
            var command = promise.ApplicationCommand;
            var commandId = command.Id;
            var commandType = command.GetType().FullName;

            if (!ExecutionPromiseDict.TryRemove(commandId, out var executionPromise))
            {
                _logger.LogWarning($"Over execution timeout upper limit: {delay} seconds, Cancelling application Command, Id:{commandId}, Type:{commandType}, but task source does not exist.");

                return;
            }

            var returnScheme = command.ReturnScheme;
            var result = ApplicationCommandResult.TimeOuted(commandId, commandType,"The execution of application command timeout, please try again.");

            _logger.LogWarning(executionPromise.TrySetResult(result)
                ? $"Over execution timeout upper limit: {delay} seconds, cancelled application Command, Id:{commandId}, Type:{commandType}, ReturnScheme:{returnScheme}."
                : $"Over execution timeout upper limit: {delay} seconds, Failed to cancel application Command, Id:{commandId}, Type:{commandType}, ReturnScheme:{returnScheme}.");
        }
    }
}