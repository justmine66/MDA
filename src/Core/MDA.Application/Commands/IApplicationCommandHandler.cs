using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandHandler<in TApplicationCommand> 
        where TApplicationCommand : IApplicationCommand
    {
        void OnApplicationCommand(IApplicationCommandContext context, TApplicationCommand command);
    }

    public interface IAsyncApplicationCommandHandler<in TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        Task OnApplicationCommandAsync(IApplicationCommandContext context, TApplicationCommand command, CancellationToken token = default);
    }
}
