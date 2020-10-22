using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class MemoryAggregateRootCheckpointManager : IAggregateRootCheckpointManager
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, AggregateRootCheckpoint> _savePoints;

        public MemoryAggregateRootCheckpointManager(ILogger<MemoryAggregateRootCheckpointManager> logger)
        {
            _logger = logger;
            _savePoints = new ConcurrentDictionary<string, AggregateRootCheckpoint>();
        }

        public async Task SnapshotCheckpointAsync<TAggregateRootId>(
            IEventSourcedAggregateRoot<TAggregateRootId> aggregateRoot,
            CancellationToken token = default)
        {
            var newSavepoint = new AggregateRootCheckpoint(aggregateRoot);

            _savePoints.AddOrUpdate(aggregateRoot.Id.ToString(), newSavepoint, (key, old) => old.Refresh(aggregateRoot));

            await Task.CompletedTask;
        }

        public async Task<AggregateRootCheckpoint> RestoreCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId) ||
                aggregateRootType == null)
            {
                return null;
            }

            if (!typeof(IEventSourcedAggregateRoot).IsAssignableFrom(aggregateRootType))
            {
                _logger.LogError($"Restoring aggregate root from memory save point has a error, reason: The {aggregateRootType.FullName} cannot assign to {nameof(IEventSourcedAggregateRoot)}.");

                return null;
            }

            if (!_savePoints.TryGetValue(aggregateRootId.ToString(), out var savePoint))
                return await Task.FromResult<AggregateRootCheckpoint>(null);

            var targetType = savePoint?.AggregateRoot?.GetType();
            if (targetType == null)
            {
                return null;
            }

            if (aggregateRootType != targetType)
            {
                _logger.LogError($"Restoring aggregate root from memory save point has a error, reason: The {aggregateRootType.FullName} cannot equal to {targetType.FullName}.");

                return null;
            }

            _logger.LogInformation($"Restored aggregate root from memory save point, {savePoint}.");

            return savePoint;
        }
    }
}
