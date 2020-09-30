using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MDA.Domain.Events;

namespace MDA.Domain.Models
{
    public interface IAggregateRootStateBackend
    {
        Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default);

        Task AppendMutatingDomainEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken token = default);
    }
}
