using System.Threading.Tasks;

namespace MDA.Commanding
{
    /// <summary>
    /// 命令服务。
    /// </summary>
    public interface ICommandService : ICommandBus
    {
        Task<CommandExcutedResult> ExcuteAsync<TCommand>(TCommand command)
            where TCommand : ICommand;
    }
}
