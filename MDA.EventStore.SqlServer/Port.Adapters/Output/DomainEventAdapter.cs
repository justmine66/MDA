using MDA.Event.Abstractions;
using MDA.EventStore.SqlServer.Poes;
using System;

namespace MDA.EventStore.SqlServer.Port.Adapters.Output
{
    public class DomainEventAdapter
    {
        public static TEvent ToDomainEvent<TEvent>(
            StoredDomainEvent storedEvent,
            IEventSerializer serializer)
            where TEvent : IDomainEvent
        {
            var eventType = default(Type);

            try
            {
                eventType = Type.GetType(storedEvent.AggregateRootTypeName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Domain event type load error, because: {0}", ex));
            }

            return serializer.Deserialize<TEvent>(storedEvent.EventBody);
        }

        public static IDomainEvent ToDomainEvent(
            StoredDomainEvent storedEvent,
            IEventSerializer serializer)
        {
            return ToDomainEvent<IDomainEvent>(storedEvent, serializer);
        }
    }
}
