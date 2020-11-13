using MDA.Domain.Events;
using MDA.Infrastructure.Serialization;
using MDA.Infrastructure.Typing;
using System;

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
                Payload = serializer.Serialize(@event),
            };

        public static IDomainEvent ToDomainEvent(
            DomainEventRecord record,
            ITypeResolver resolver,
            IBinarySerializer binarySerializer)
        {
            if (!resolver.TryResolveType(record.DomainEventType, out var domainEventType))
            {
                throw new TypeLoadException($"Cannot resolve type: {record.DomainEventType}.");
            }

            if (!(typeof(IDomainEvent).IsAssignableFrom(domainEventType)))
            {
                throw new InvalidOperationException($"The {record.DomainEventType} cannot assign to {typeof(IDomainEvent).FullName}.");
            }

            var obj = binarySerializer.Deserialize(record.Payload, domainEventType);

            return obj is IDomainEvent domainEvent ? domainEvent : null;
        }
    }
}
