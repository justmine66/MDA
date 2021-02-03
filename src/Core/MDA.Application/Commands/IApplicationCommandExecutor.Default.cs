using MDA.Domain;
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
        private readonly IApplicationCommandResultListener _commandResultListener;

        public DefaultApplicationCommandExecutor(
            IMessagePublisher messagePublisher,
            IOptions<ApplicationCommandOptions> options,
            IApplicationCommandResultListener commandResultListener)
        {
            _options = options.Value;
            _messagePublisher = messagePublisher;
            _commandResultListener = commandResultListener;
        }

        public ApplicationCommandResult ExecuteCommand(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
        {
            return ExecuteCommandAsync(command, returnScheme)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public ApplicationCommandResult<TPayload> ExecuteCommand<TPayload>(IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
        {
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

            await PublishAsync(command, returnScheme, token);

            var promise = new ApplicationCommandExecutionPromise(command);

            _commandResultListener.AddExecutingPromise(promise);

            return await promise.Future.ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command, CancellationToken token = default)
            => await ExecuteCommandAsync<TPayload>(command, ApplicationCommandResultReturnSchemes.OnDomainCommandHandled, token);

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default)
        {
            PreConditions.NotNull(command, nameof(command));

            await PublishAsync(command, returnScheme, token);

            var promise = new ApplicationCommandExecutionPromise<TPayload>(command);

            _commandResultListener.AddExecutingPromise(promise);

            return await promise.Future.ConfigureAwait(false);
        }

        private async Task PublishAsync(IApplicationCommand command, ApplicationCommandResultReturnSchemes returnScheme, CancellationToken token = default)
        {
            command.ReturnScheme = returnScheme;
            command.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(command, token).ConfigureAwait(false);
        }
    }
}
