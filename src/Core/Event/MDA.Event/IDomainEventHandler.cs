using System.Threading.Tasks;

namespace MDA.Event
{
    public interface IDomainEventHandler
    {
        void Handle(DomainEvent @event);
    }

    public interface IDomainEventHandler<in TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        void Handle(TDomainEvent @event);
    }
}
