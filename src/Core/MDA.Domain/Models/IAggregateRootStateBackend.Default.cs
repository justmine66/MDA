using MDA.Domain.Events;
using MDA.Shared.Utils;
using Microsoft.Extensions.Logging;
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
        private readonly IAggregateRootFactory _aggregateRootFactory;

        public DefaultAggregateRootStateBackend(
            IDomainEventStateBackend stateBackend,
            IAggregateRootCheckpointManager checkpointManager,
            ILogger<DefaultAggregateRootStateBackend> logger,
            IAggregateRootMemoryCache memoryCache,
            IAggregateRootFactory aggregateRootFactory)
        {
            _eventStateBackend = stateBackend;
            _checkpointManager = checkpointManager;
            _logger = logger;
            _memoryCache = memoryCache;
            _aggregateRootFactory = aggregateRootFactory;
        }

        public async Task<IEventSourcedAggregateRoot> GetAsync<TAggregateRootId>(
            TAggregateRootId aggregateRootId,
            Type aggregateRootType,
            CancellationToken token = default)
        {
            // 1. 获取检查点
            var checkPoint = await _checkpointManager.RestoreCheckpointAsync(aggregateRootId.ToString(), aggregateRootType, token);

            // 2. 获取从检查点开始产生的事件流
            var aggregateRoot = checkPoint == null
                ? _aggregateRootFactory.CreateAggregateRoot(aggregateRootId, aggregateRootType)
                : checkPoint.AggregateRoot;

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

        public async Task<IEnumerable<DomainEventResult>> AppendMutatingDomainEventsAsync(IEnumerable<IDomainEvent> events, CancellationToken token = default)
            => await _eventStateBackend.AppendAsync(events, token);
    }
}
