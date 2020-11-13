using MDA.Domain.Events;
using MDA.Infrastructure.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootCheckpointManager : IAggregateRootCheckpointManager
    {
        private readonly CheckpointTriggerOptions _options;
        private readonly IDomainEventStateBackend _eventStateBackend;
        private readonly ILogger<DefaultAggregateRootCheckpointManager> _logger;
        private readonly IAggregateRootCheckpointStateBackend<IEventSourcedAggregateRoot> _checkpointStateBackend;

        public DefaultAggregateRootCheckpointManager(
            IOptions<CheckpointTriggerOptions> options,
            IDomainEventStateBackend eventStateBackend,
            ILogger<DefaultAggregateRootCheckpointManager> logger,
            IAggregateRootCheckpointStateBackend<IEventSourcedAggregateRoot> backend)
        {
            _logger = logger;
            _options = options.Value;
            _checkpointStateBackend = backend;
            _eventStateBackend = eventStateBackend;
        }

        public async Task CheckpointAsync(
            IEventSourcedAggregateRoot aggregateRoot,
            CancellationToken token = default)
        {
            var aggregateRootId = aggregateRoot.Id;
            var aggregateRootType = aggregateRoot.GetType().FullName;
            var aggregateGeneration = aggregateRoot.Generation;
            var aggregateVersion = aggregateRoot.Version;

            var metrics = await _eventStateBackend.StatMetricsAsync(aggregateRootId, aggregateGeneration, token);
            if (metrics.TriggerCheckpoint(_options))
            {
                var nextGeneration = ++aggregateRoot.Generation;
                var checkpoint = new AggregateRootCheckpoint<IEventSourcedAggregateRoot>(aggregateRoot.Id, aggregateRoot.GetType(), nextGeneration, aggregateRoot.Version, aggregateRoot);

                var message = $"id: {aggregateRootId}, Type: {aggregateRootType}, Generation: {nextGeneration}, Version: {aggregateVersion}, UnCheckpointedBytes: {metrics.UnCheckpointedBytes} >= {_options.UnCheckpointedBytes}, UnCheckpointedCount: {metrics.UnCheckpointedCount} >= {_options.UnCheckpointedCount}";

                try
                {
                    await _checkpointStateBackend.AppendAsync(checkpoint, token);
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"Checkpointing the aggregate root, {message} has a unknown exception: {LogFormatter.PrintException(e)}.");

                    return;
                }

                _logger.LogInformation($"Checkpointed the aggregate root, {message}.");
            }
            else
            {
                _logger.LogInformation($"No triggering checkpoint for the aggregate root, id: {aggregateRootId}, Type: {aggregateRootType}, Generation: {aggregateGeneration}, Version: {aggregateVersion}, UnCheckpointedBytes: {metrics.UnCheckpointedBytes} < {_options.UnCheckpointedBytes}, UnCheckpointedCount: {metrics.UnCheckpointedCount} < {_options.UnCheckpointedCount}.");
            }
        }

        public async Task<AggregateRootCheckpoint<IEventSourcedAggregateRoot>> GetLatestCheckpointAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default)
            => await _checkpointStateBackend.GetLatestCheckpointAsync(aggregateRootId, aggregateRootType, token);
    }
}
