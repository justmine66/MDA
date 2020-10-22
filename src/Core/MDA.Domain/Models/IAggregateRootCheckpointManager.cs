using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootCheckpointManager
    {
        Task SnapshotCheckpointAsync<TAggregateRootId>(
            IEventSourcedAggregateRoot<TAggregateRootId> aggregateRoot,
            CancellationToken token = default);

        Task<AggregateRootCheckpoint> RestoreCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);
    }
}
