using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootCheckpointManager
    {
        Task SnapshotCheckpointAsync(
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token = default);

        Task<AggregateRootCheckpoint<IEventSourcedAggregateRoot>> RestoreCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);

        Task<IEventSourcedAggregateRoot> RestoreAggregateRootAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);
    }
}
