using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class ApplicationCommandExecutor : IApplicationCommandExecutor
    {
        public ApplicationCommandResult ExecuteCommand(IApplicationCommand command)
        {
            throw new System.NotImplementedException();
        }

        public ApplicationCommandResult<TResult> ExecuteCommand<TResult>(IApplicationCommand command)
        {
            throw new System.NotImplementedException();
        }

        public ApplicationCommandResult<TResult, TCommandId> ExecuteCommand<TResult, TCommandId>(IApplicationCommand<TCommandId> command)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(IApplicationCommand command, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationCommandResult<TResult, TCommandId>> ExecuteCommandAsync<TResult, TCommandId>(IApplicationCommand<TCommandId> command, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
