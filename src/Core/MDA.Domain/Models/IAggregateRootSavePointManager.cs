using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public interface IAggregateRootSavePointManager
    {
        Task SnapshotSavePointAsync(
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token = default);

        Task<AggregateRootSavePoint<IEventSourcedAggregateRoot>> RestoreSavePointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);

        Task<IEventSourcedAggregateRoot> RestoreAggregateRootAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default);
    }
}
