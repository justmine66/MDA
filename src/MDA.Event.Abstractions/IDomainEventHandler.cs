using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventHandler
    {
    }

    public interface IDomainEventHandler<in TIDomainEvent>
        : IDomainEventHandler
        where TIDomainEvent : IDomainEvent
    {
        Task HandleAsync(IDomainEvent @event);
    }
}
