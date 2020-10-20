using MDA.Domain.Events;
using MDA.Shared.Serialization;
using MDA.Shared.Types;

namespace MDA.StateBackend.MySql
{
    public static class DomainEventRecordPortAdapter
    {
        public static DomainEventRecord ToDomainEventRecord(
            IDomainEvent @event,
            IBinarySerializer serializer)
            => new DomainEventRecord()
            {
                DomainCommandId = @event.DomainCommandId,
                DomainCommandType = @event.DomainCommandType.FullName,
                DomainCommandVersion = @event.DomainCommandVersion,
                AggregateRootVersion = @event.AggregateRootVersion,
                AggregateRootId = @event.AggregateRootId,
                AggregateRootType = @event.AggregateRootType.FullName,
                AggregateRootGeneration = @event.AggregateRootGeneration,
                CreatedTimestamp = @event.Timestamp,
                DomainEventId = @event.Id,
                DomainEventType = @event.GetType().FullName,
                DomainEventVersion = @event.Version,
                Payload = serializer.Serialize(@event)
            };

        public static IDomainEvent ToDomainEvent(
            DomainEventRecord record,
            ITypeResolver resolver,
            IBinarySerializer binarySerializer)
        {
            if (!resolver.TryResolveType(record.DomainEventType, out var domainEventType))
            {
                return null;
            }

            if (!(typeof(IDomainEvent).IsAssignableFrom(domainEventType)))
            {
                return null;
            }

            var obj = binarySerializer.Deserialize(record.Payload, domainEventType);

            return obj is IDomainEvent domainEvent ? domainEvent : null;
        }
    }
}
