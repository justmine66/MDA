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
                Payload = serializer.Serialize(@event)
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

            if (!(typeof(IDomainEvent).IsAssignableFrom(domainEventType)))
            {
                return null;
            }

            return serializer.DeSerialize<IDomainEvent>(record.Payload);
        }
    }
}
