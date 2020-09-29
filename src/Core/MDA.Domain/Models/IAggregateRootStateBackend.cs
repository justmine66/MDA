using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootStateBackend
    {
        Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default);
    }
}
