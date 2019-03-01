using MDA.Event;
using MDA.EventStore.MySql.Poes;

namespace MDA.EventStore.MySql.Port.Adapters.Output
{
    public class DomainEventAdapter
    {
        public static TEvent ToDomainEvent<TEvent>(
            StoredDomainEvent storedEvent,
            IEventSerializer serializer)
            where TEvent : DomainEvent
        {
            return serializer.Deserialize<TEvent>(storedEvent.EventBody);
        }

        public static DomainEvent ToDomainEvent(
            StoredDomainEvent storedEvent,
            IEventSerializer serializer)
        {
            return ToDomainEvent<DomainEvent>(storedEvent, serializer);
        }
    }
}
