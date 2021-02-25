using System.Threading;
using System.Threading.Tasks;
using MDA.Domain;
using MDA.Domain.Shared;

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
            ApplicationCommandReplySchemes replyScheme = ApplicationCommandReplySchemes.OnDomainCommandHandled)
            => _executor.ExecuteCommand(command, replyScheme);

        public ApplicationCommandResult<TResult> ExecuteCommand<TResult>(
            IApplicationCommand command,
            ApplicationCommandReplySchemes replyScheme = ApplicationCommandReplySchemes.OnDomainCommandHandled)
            => _executor.ExecuteCommand<TResult>(command, replyScheme);

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default)
        {
            return await ExecuteCommandAsync(command, ApplicationCommandReplySchemes.OnDomainCommandHandled, token)
                .ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(
            IApplicationCommand command,
            ApplicationCommandReplySchemes replyScheme = ApplicationCommandReplySchemes.OnDomainCommandHandled,
            CancellationToken token = default)
            => await _executor.ExecuteCommandAsync(command, replyScheme, token);

        public async Task<ApplicationCommandResult<TPayload>> ExecuteCommandAsync<TPayload>(IApplicationCommand command, CancellationToken token = default)
        {
            return await ExecuteCommandAsync<TPayload>(command, ApplicationCommandReplySchemes.OnDomainCommandHandled, token)
                .ConfigureAwait(false);
        }

        public async Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(
            IApplicationCommand command,
            ApplicationCommandReplySchemes replyScheme = ApplicationCommandReplySchemes.OnDomainCommandHandled,
            CancellationToken token = default)
            => await _executor.ExecuteCommandAsync<TResult>(command, replyScheme, token);
    }
}