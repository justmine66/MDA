using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandExecutor
    {
        ApplicationCommandResult ExecuteCommand(IApplicationCommand command);
        ApplicationCommandResult<TResult> ExecuteCommand<TResult>(IApplicationCommand command);
        ApplicationCommandResult<TResult, TCommandId> ExecuteCommand<TResult, TCommandId>(IApplicationCommand<TCommandId> command);
    }

    public interface IAsyncApplicationCommandExecutor : IApplicationCommandExecutor
    {
        Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command, CancellationToken token = default);
        Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(IApplicationCommand command, CancellationToken token = default);
        Task<ApplicationCommandResult<TResult, TCommandId>> ExecuteCommandAsync<TResult, TCommandId>(IApplicationCommand<TCommandId> command, CancellationToken token = default);
    }
}
