using MDA.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Event
{
    public interface IDomainEventStore
    {
        Task<AsyncResult<int>> CountStoredEventsAsync();

        Task<AsyncResult<IEnumerable<DomainEvent>>> GetAllAggregateStoredEventsSinceAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int sequence);

        Task<AsyncResult<IEnumerable<DomainEvent>>> GetAllAggregateStoredEventsBetweenAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int lowSequence,
            int highSequence);

        Task<AsyncResult<DomainEventAppendResult>> AppendAsync(DomainEvent domainEvent);

        Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(DomainEventStream eventStream);
    }
}
