using MDA.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Models
{
    public class DefaultAggregateRootStateBackend : IAggregateRootStateBackend
    {
        private readonly IAggregateRootMemoryCache _memoryCache;
        private readonly IDomainEventStateBackend _eventStateBackend;
        private readonly IAggregateRootSavePointManager _savePointManager;

        public DefaultAggregateRootStateBackend(
            IDomainEventStateBackend stateBackend,
            IAggregateRootMemoryCache memoryCache,
            IAggregateRootSavePointManager savePointManager)
        {
            _eventStateBackend = stateBackend;
            _memoryCache = memoryCache;
            _savePointManager = savePointManager;
        }

        public async Task<IEventSourcedAggregateRoot> GetAsync(string aggregateRootId, CancellationToken token = default)
        {
            // 1. 获取保存点
            var savePoint = await _savePointManager.RestoreSavePointAsync<IEventSourcedAggregateRoot>(token);
            var aggregateRoot = savePoint.AggregateRoot;

            // 2. 获取从保存点开始产生的事件流
            var eventStream = await _eventStateBackend.GetEventStreamAsync(aggregateRoot.Id, savePoint.DomainEventOffset, token);

            // 3. 重放事件
            aggregateRoot.ReplayDomainEvents(eventStream);

            // 4. 设置缓存
            _memoryCache.Set(aggregateRoot);

            return aggregateRoot;
        }
    }
}
