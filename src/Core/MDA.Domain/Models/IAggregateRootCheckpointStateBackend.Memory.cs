using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MDA.Infrastructure.Serialization;

namespace MDA.Domain.Models
{
    public class MemoryAggregateRootCheckpointStateBackend<TPayload> : IAggregateRootCheckpointStateBackend<TPayload> where TPayload : ISerializationMetadataProvider
    {
        private readonly ILogger _logger;
        private readonly List<AggregateRootCheckpoint<TPayload>> _checkpoints;

        public MemoryAggregateRootCheckpointStateBackend(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(typeof(MemoryAggregateRootCheckpointStateBackend<TPayload>));
            _checkpoints = new List<AggregateRootCheckpoint<TPayload>>();
        }

        public async Task<AggregateRootCheckpointResult> AppendAsync(AggregateRootCheckpoint<TPayload> checkpoint, CancellationToken token = default)
        {
            _checkpoints.Add(checkpoint);

            var result = AggregateRootCheckpointResult.StorageSucceed(checkpoint.AggregateRootId);

            _logger.LogInformation($"Checkpointed to memory state backend, {checkpoint}.");

            return await Task.FromResult(result);
        }

        public Task<AggregateRootCheckpoint<TPayload>> GetLatestCheckpointAsync(
            string aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            var checkpoint = _checkpoints.LastOrDefault(
                it => it.AggregateRootId == aggregateRootId &&
                      it.AggregateRootType == aggregateRootType);

            return Task.FromResult(checkpoint);
        }
    }
}
