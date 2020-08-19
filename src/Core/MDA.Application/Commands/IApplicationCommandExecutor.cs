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
        Task<ApplicationCommandResult> ExecuteCommandAsync(IApplicationCommand command);
        Task<ApplicationCommandResult<TResult>> ExecuteCommandAsync<TResult>(IApplicationCommand command);
        Task<ApplicationCommandResult<TResult, TCommandId>> ExecuteCommandAsync<TResult, TCommandId>(IApplicationCommand<TCommandId> command);
    }
}
