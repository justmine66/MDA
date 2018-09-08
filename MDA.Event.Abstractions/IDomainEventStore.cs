using MDA.Common;
using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventStore
    {
        Task<int> CountStoredEventsAsync();

        Task<IDomainEvent[]> GetAllAggregateStoredEventsSinceAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int sequence);

        Task<IDomainEvent[]> GetAllAggregateStoredEventsBetweenAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            long lowSequence,
            long highSequence);

        Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent);

        Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(DomainEventStream eventStream);
    }
}
