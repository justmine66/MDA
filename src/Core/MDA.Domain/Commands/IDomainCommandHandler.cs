using MDA.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Commands
{
    public interface IDomainCommandHandler<in TDomainCommand> : IMessageHandler<TDomainCommand>
        where TDomainCommand : IDomainCommand
    {
        new void Handle(TDomainCommand command);
    }

    public interface IAsyncDomainCommandHandler<in TDomainCommand> : IAsyncMessageHandler<TDomainCommand>
        where TDomainCommand : IDomainCommand
    {
        new Task HandleAsync(TDomainCommand command, CancellationToken token = default);
    }
}
