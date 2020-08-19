using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public interface IApplicationCommandHandler<in TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        void Handle(IApplicationCommandContext context, TApplicationCommand command);
    }

    public interface IAsyncApplicationCommandHandler<in TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        Task HandleAsync(IApplicationCommandContext context, TApplicationCommand command);
    }
}
