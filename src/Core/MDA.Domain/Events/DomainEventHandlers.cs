using MDA.MessageBus;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent> : IMessageHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        new void Handle(TDomainEvent @event);
    }

    public interface IAsyncDomainEventHandler<in TDomainEvent> : IAsyncMessageHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        new Task HandleAsync(TDomainEvent @event, CancellationToken token = default);
    }
}
