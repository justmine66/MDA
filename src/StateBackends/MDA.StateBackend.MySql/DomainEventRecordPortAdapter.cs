using System;
using MDA.Domain.Events;
using MDA.Shared.Serialization;
using MDA.Shared.Types;

namespace MDA.StateBackend.MySql
{
    public static class DomainEventRecordPortAdapter
    {
        public static DomainEventRecord ToDomainEventRecord(IDomainEvent @event, IBinarySerializer serializer)
            => new DomainEventRecord()
            {
                DomainCommandId = @event.DomainCommandId,
                DomainCommandTypeFullName = @event.DomainCommandType.FullName,
                DomainCommandVersion = @event.DomainCommandVersion,
                AggregateRootVersion = @event.AggregateRootVersion,
                AggregateRootId = @event.AggregateRootId,
                AggregateRootTypeFullName = @event.AggregateRootType.FullName,
                Id = @event.Id,
                TypeFullName = @event.GetType().FullName,
                Version = @event.Version,
                Payload = serializer.Serialize(@event.Payload)
            };

        public static IDomainEvent ToDomainEvent(
            DomainEventRecord record,
            ITypeResolver resolver,
            IBinarySerializer serializer)
        {
            if (!resolver.TryResolveType(record.TypeFullName, out var domainEventType))
            {
                return null;
            }

            if (!(Activator.CreateInstance(domainEventType) is IDomainEvent domainEvent))
            {
                return null;
            }

            domainEvent.DomainCommandId = record.DomainCommandId;
            domainEvent.DomainCommandType = resolver.TryResolveType(record.DomainCommandTypeFullName, out var domainCommandType)
                    ? domainCommandType
                    : null;
            domainEvent.DomainCommandVersion = record.DomainCommandVersion;
            domainEvent.AggregateRootVersion = record.AggregateRootVersion;
            domainEvent.AggregateRootId = record.AggregateRootId;
            domainEvent.AggregateRootType = resolver.TryResolveType(record.AggregateRootTypeFullName, out var aggregateRootType)
                ? aggregateRootType
                : null;
            domainEvent.Id = record.Id;
            domainEvent.Version = record.Version;
            domainEvent.Payload = serializer.DeSerialize<object>(record.Payload);

            return domainEvent;
        }
    }
}
