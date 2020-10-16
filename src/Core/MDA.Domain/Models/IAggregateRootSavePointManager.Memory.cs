using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class MemoryAggregateRootSavePointManager : IAggregateRootSavePointManager
    {
        private readonly ILogger _logger;
        private readonly IAggregateRootFactory _aggregateRootFactory;
        private readonly ConcurrentDictionary<string, AggregateRootSavePoint<IEventSourcedAggregateRoot>> _savePoints;

        public MemoryAggregateRootSavePointManager(
            ILogger<MemoryAggregateRootSavePointManager> logger,
            IAggregateRootFactory aggregateRootFactory)
        {
            _logger = logger;
            _aggregateRootFactory = aggregateRootFactory;
            _savePoints = new ConcurrentDictionary<string, AggregateRootSavePoint<IEventSourcedAggregateRoot>>();
        }

        public async Task SnapshotSavePointAsync(
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token = default)
        {
            var newSavepoint = new AggregateRootSavePoint<IEventSourcedAggregateRoot>(aggregateRoot);

            _savePoints.AddOrUpdate(aggregateRoot.Id, newSavepoint, (key, old) => old.Refresh(aggregateRoot));

            await Task.CompletedTask;
        }

        public async Task<AggregateRootSavePoint<IEventSourcedAggregateRoot>> RestoreSavePointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId))
            {
                return null;
            }

            if (aggregateRootType == null)
            {
                return null;
            }

            if (!typeof(IEventSourcedAggregateRoot).IsAssignableFrom(aggregateRootType))
            {
                _logger.LogError($"Restoring aggregate root from memory save point has a error, reason: The {aggregateRootType.FullName} cannot assign to {nameof(IEventSourcedAggregateRoot)}.");

                return null;
            }

            if (_savePoints.TryGetValue(aggregateRootId, out var savePoint))
            {
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

            var aggregateRoot = _aggregateRootFactory.CreateAggregateRoot(aggregateRootId, aggregateRootType);
            if (aggregateRoot == null)
            {
                return null;
            }

            var savepoint = new AggregateRootSavePoint<IEventSourcedAggregateRoot>(aggregateRoot);

            return await Task.FromResult(savepoint);
        }

        public async Task<IEventSourcedAggregateRoot> RestoreAggregateRootAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId))
            {
                return null;
            }

            var savepoint = await RestoreSavePointAsync(aggregateRootId, aggregateRootType, token);

            return savepoint?.AggregateRoot;
        }
    }
}
