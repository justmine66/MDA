using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandHandler<in TApplicationCommand> 
        where TApplicationCommand : IApplicationCommand
    {
        void OnApplicationCommand(IApplicationCommandingContext context, TApplicationCommand command);
    }

    public interface IAsyncApplicationCommandHandler<in TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        Task OnApplicationCommandAsync(IApplicationCommandingContext context, TApplicationCommand command, CancellationToken token = default);
    }
}
