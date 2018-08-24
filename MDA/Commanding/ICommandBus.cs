using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Commanding
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task SendAllAsync<TCommand>(IEnumerable<TCommand> command)
            where TCommand : ICommand;
    }
}
