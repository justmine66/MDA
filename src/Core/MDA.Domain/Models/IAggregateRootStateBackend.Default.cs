using MDA.Domain.Events;
using MDA.Shared.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootStateBackend : IAggregateRootStateBackend
    {
        private readonly ILogger _logger;
        
        private readonly IAggregateRootMemoryCache _memoryCache;
        private readonly IDomainEventStateBackend _eventStateBackend;
        private readonly IAggregateRootCheckpointManager _checkpointManager;
        private readonly AggregateRootStateBackendOptions _options;

        public DefaultAggregateRootStateBackend(
            IDomainEventStateBackend stateBackend,
            IAggregateRootCheckpointManager checkpointManager,
            ILogger<DefaultAggregateRootStateBackend> logger,
            IOptions<AggregateRootStateBackendOptions> options,
            IAggregateRootMemoryCache memoryCache)
        {
            _eventStateBackend = stateBackend;
            _checkpointManager = checkpointManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _options = options.Value;
        }

        public async Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default)
        {
            // 1. 获取检查点
            var checkPoint = await _checkpointManager.RestoreCheckpointAsync(aggregateRootId, aggregateRootType, token);
            var aggregateRoot = checkPoint?.AggregateRoot;
            if (aggregateRoot == null)
            {
                return null;
            }

            // 2. 获取从检查点开始产生的事件流
            var eventStream = await _eventStateBackend.GetEventStreamAsync(
                aggregateRoot.Id,
                aggregateRoot.Generation,
                aggregateRoot.Version,
                token);

            if (eventStream.IsEmpty())
            {
                _memoryCache.Set(aggregateRoot);

                return aggregateRoot;
            }

            // 3. 重放事件
            try
            {
                aggregateRoot.ReplayDomainEvents(eventStream);
            }
            catch (Exception e)
            {
                _logger.LogError($"Replay domain event for aggregateRoot[Id: {aggregateRootId}, Type: {aggregateRootType.FullName}] has a error, reason: {e}.");

                return null;
            }

            _memoryCache.Set(aggregateRoot);

            return aggregateRoot;
        }

        public async Task AppendMutatingDomainEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken token = default)
        {
            var batchSize = _options.SubmitBatchSize;
            var duration = _options.SubmitDurationInMilliseconds;

            // todo: 实现按批、超时提交。
            var results = await _eventStateBackend.AppendAsync(events, token);
        }
    }
}
