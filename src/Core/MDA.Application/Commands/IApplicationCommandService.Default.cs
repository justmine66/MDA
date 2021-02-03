using System.Threading;
using System.Threading.Tasks;
using MDA.Domain;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandService : IApplicationCommandService
    {
        private readonly IApplicationCommandPublisher _publisher;
        private readonly IApplicationCommandExecutor _executor;

        public DefaultApplicationCommandService(
            IApplicationCommandPublisher publisher,
            IApplicationCommandExecutor executor)
        {
            _publisher = publisher;
            _executor = executor;
        }

        public void Publish(IApplicationCommand command)
            => _publisher.Publish(command);

        public async Task PublishAsync(IApplicationCommand command, CancellationToken token = default)
            => await _publisher.PublishAsync(command, token);

        public ApplicationCommandResult ExecuteCommand(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
            => _executor.ExecuteCommand(command, returnScheme);

        public ApplicationCommandResult<TResult> ExecuteCommand<TResult>(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled)
            => _executor.ExecuteCommand<TResult>(command, returnScheme);

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default)
        {
            return await ExecuteCommandAsync(command, ApplicationCommandResultReturnSchemes.OnDomainCommandHandled, token)
                .ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default)
            => await _executor.ExecuteCommandAsync(command, returnScheme, token);

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command, CancellationToken token = default)
        {
            return await ExecuteCommandAsync<TPayload>(command, ApplicationCommandResultReturnSchemes.OnDomainCommandHandled, token)
                .ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(
            IApplicationCommand command,
            ApplicationCommandResultReturnSchemes returnScheme = ApplicationCommandResultReturnSchemes.OnDomainCommandHandled,
            CancellationToken token = default)
            => await _executor.ExecuteCommandAsync<TResult>(command, returnScheme, token);
    }
}