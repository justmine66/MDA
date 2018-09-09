using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventHandler
    {
        void Handle(IDomainEvent @event);
    }

    public interface IDomainEventHandler<in TIDomainEvent>
        where TIDomainEvent : IDomainEvent
    {
        void Handle(TIDomainEvent @event);
    }
}
