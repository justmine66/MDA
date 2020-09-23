using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class MemoryAggregateRootSavePointManager : IAggregateRootSavePointManager
    {
        private readonly ConcurrentDictionary<string, IEventSourcedAggregateRoot> _savePoints;

        public MemoryAggregateRootSavePointManager()
        {
            _savePoints = new ConcurrentDictionary<string, IEventSourcedAggregateRoot>();
        }

        public async Task SnapshotSavePointAsync(CancellationToken token = default)
        {
            await Task.CompletedTask;
        }

        public async Task<AggregateRootSavePoint<TAggregateRoot>> RestoreSavePointAsync<TAggregateRoot>(CancellationToken token = default)
            where TAggregateRoot : IEventSourcedAggregateRoot
        {
            var instance = Activator.CreateInstance<TAggregateRoot>();
            var savepoint = new AggregateRootSavePoint<TAggregateRoot>(instance);

            return await Task.FromResult(savepoint);
        }
    }
}
