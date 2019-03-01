using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Command
{
    /// <summary>
    /// 命令总线
    /// </summary>
    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : Command;

        Task SendAllAsync<TCommand>(IEnumerable<TCommand> commands)
            where TCommand : Command;
    }
}
