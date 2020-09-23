using MDA.Shared.DataStructures;
using Microsoft.Extensions.Options;

namespace MDA.Domain.Models
{
    public class LruAggregateRootMemoryCache : IAggregateRootMemoryCache
    {
        private readonly LRUCache<string, IEventSourcedAggregateRoot> _cache;

        public LruAggregateRootMemoryCache(IOptions<AggregateRootCacheOptions> options) 
            => _cache = new LRUCache<string, IEventSourcedAggregateRoot>(options.Value.MaxSize, options.Value.ToTimeSpan());

        public IEventSourcedAggregateRoot Get(string aggregateRootId)
        {
            if (string.IsNullOrWhiteSpace(aggregateRootId))
            {
                return null;
            }

            return _cache.TryGetValue(aggregateRootId, out var value) ? value : null;
        }

        public void Set(IEventSourcedAggregateRoot aggregateRoot) => _cache.Add(aggregateRoot.Id, aggregateRoot);

        public void Refresh(string aggregateRootId) => Get(aggregateRootId);

        public void Clear() => _cache.Clear();
    }
}
