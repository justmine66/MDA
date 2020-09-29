using MDA.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootStateBackend : IAggregateRootStateBackend
    {
        private readonly ILogger _logger;
        private readonly IAggregateRootMemoryCache _memoryCache;
        private readonly IDomainEventStateBackend _eventStateBackend;
        private readonly IAggregateRootSavePointManager _savePointManager;

        public DefaultAggregateRootStateBackend(
            IDomainEventStateBackend stateBackend,
            IAggregateRootMemoryCache memoryCache,
            IAggregateRootSavePointManager savePointManager, 
            ILogger<DefaultAggregateRootStateBackend> logger)
        {
            _eventStateBackend = stateBackend;
            _memoryCache = memoryCache;
            _savePointManager = savePointManager;
            _logger = logger;
        }

        public async Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, Type aggregateRootType, CancellationToken token = default)
        {
            // 1. 获取保存点
            var savePoint = await _savePointManager.RestoreSavePointAsync(aggregateRootId, aggregateRootType, token);
            var aggregateRoot = savePoint?.AggregateRoot;
            if (aggregateRoot == null)
            {
                return null;
            }

            // 2. 获取从保存点开始产生的事件流
            var eventStream = await _eventStateBackend.GetEventStreamAsync(aggregateRoot.Id, aggregateRoot.Version, token);

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
           

            // 4. 设置缓存
            _memoryCache.Set(aggregateRoot);

            return aggregateRoot;
        }
    }
}
