using System.Threading;
using System.Threading.Tasks;

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

        public ApplicationCommandResult ExecuteCommand(IApplicationCommand command) 
            => _executor.ExecuteCommand(command);

        public ApplicationCommandResult<TResult> ExecuteCommand<TResult>(IApplicationCommand command)
            => _executor.ExecuteCommand<TResult>(command);

        public ApplicationCommandResult<TResult, TCommandId> ExecuteCommand<TResult, TCommandId>(IApplicationCommand<TCommandId> command)
            => _executor.ExecuteCommand<TResult, TCommandId>(command);

        public async Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default)
            => await _executor.ExecuteCommandAsync(command,token);

        public async Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(IApplicationCommand command, CancellationToken token = default)
            => await _executor.ExecuteCommandAsync<TResult>(command, token);

        public async Task<ApplicationCommandResult<TResult, TCommandId>> ExecuteCommandAsync<TResult, TCommandId>(IApplicationCommand<TCommandId> command, CancellationToken token = default)
            => await _executor.ExecuteCommandAsync<TResult, TCommandId>(command, token);
    }
}
