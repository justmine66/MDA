using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventHandler
    {
        Task HandleAsync(IDomainEvent @event);
    }

    public interface IDomainEventHandler<in TIDomainEvent>
        : IDomainEventHandler
        where TIDomainEvent : IDomainEvent
    {
        Task HandleAsync(TIDomainEvent @event);
    }
}
