using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        void Handle(TDomainEvent @event);
    }

    public interface IAsyncDomainEventHandler<in TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
        Task HandleAsync(TDomainEvent @event);
    }
}
