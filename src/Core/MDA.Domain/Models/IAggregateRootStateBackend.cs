using MDA.Domain.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootStateBackend
    {
        Task<IEventSourcedAggregateRoot> GetAsync<TAggregateRootId>(
            TAggregateRootId aggregateRootId, 
            Type aggregateRootType,
            CancellationToken token = default);

        Task<IEnumerable<DomainEventResult>> AppendMutatingDomainEventsAsync(
            IEnumerable<IDomainEvent> events, 
            CancellationToken token = default);
    }
}
