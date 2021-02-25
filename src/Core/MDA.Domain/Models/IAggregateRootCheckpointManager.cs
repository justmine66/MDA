using MDA.Domain.Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootCheckpointManager
    {
        Task CheckpointAsync(
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token = default);

        Task<AggregateRootCheckpoint<IEventSourcedAggregateRoot>> GetLatestCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);
    }
}
