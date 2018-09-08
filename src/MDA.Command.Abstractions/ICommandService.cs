using System.Threading.Tasks;

namespace MDA.Command.Abstractions
{
    /// <summary>
    /// 命令服务。
    /// </summary>
    public interface ICommandService : ICommandBus
    {
        Task<CommandResult> ExcuteAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
