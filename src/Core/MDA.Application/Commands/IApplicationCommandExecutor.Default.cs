using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandExecutor : IApplicationCommandExecutor
    {
        private readonly ApplicationCommandOptions _options;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IApplicationCommandResultProcessor _commandResultProcessor;

        public DefaultApplicationCommandExecutor(
            IMessagePublisher messagePublisher,
            IOptions<ApplicationCommandOptions> options,
            IApplicationCommandResultProcessor commandResultProcessor)
        {
            _options = options.Value;
            _messagePublisher = messagePublisher;
            _commandResultProcessor = commandResultProcessor;
        }

        public ApplicationCommandResult ExecuteCommand(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
        {
            command.ReturnScheme = returnScheme;

            return ExecuteCommandAsync(command, returnScheme)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public ApplicationCommandResult<TPayload> ExecuteCommand<TPayload>(IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
        {
            command.ReturnScheme = returnScheme;

            return ExecuteCommandAsync<TPayload>(command, returnScheme)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default) 
            => await ExecuteCommandAsync(command, ApplicationCommandResultReturnSchemes.OnDomainCommandHandled, token);

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default)
        {
            PreConditions.NotNull(command, nameof(command));

            await PublishAsync(command, token);

            command.ReturnScheme = returnScheme;

            var promise = new ApplicationCommandExecutionPromise(command);

            _commandResultProcessor.AddExecutionPromise(promise);

            return await promise.Future.ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command, CancellationToken token = default)
            => await ExecuteCommandAsync<TPayload>(command, ApplicationCommandResultReturnSchemes.OnDomainCommandHandled, token);

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default)
        {
            PreConditions.NotNull(command, nameof(command));

            await PublishAsync(command, token);

            command.ReturnScheme = returnScheme;

            var promise = new ApplicationCommandExecutionPromise<TPayload>(command);

            _commandResultProcessor.AddExecutionPromise(promise);

            return await promise.Future.ConfigureAwait(false);
        }

        private void Publish(IApplicationCommand command)
        {
            command.Topic = _options.Topic;

            _messagePublisher.Publish(command);
        }

        private async Task PublishAsync(IApplicationCommand command, CancellationToken token = default)
        {
            command.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(command, token).ConfigureAwait(false);
        }
    }
}
