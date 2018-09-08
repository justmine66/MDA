using MDA.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IDomainEventStore
    {
        Task<AsyncResult<int>> CountStoredEventsAsync();

        Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsSinceAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int sequence);

        Task<AsyncResult<IEnumerable<IDomainEvent>>> GetAllAggregateStoredEventsBetweenAsync(
            string aggregateRootId,
            string aggregateRootTypeName,
            int lowSequence,
            int highSequence);

        Task<AsyncResult<DomainEventAppendResult>> AppendAsync(IDomainEvent domainEvent);

        Task<AsyncResult<DomainEventAppendResult>> AppendAllAsync(DomainEventStream eventStream);
    }
}
