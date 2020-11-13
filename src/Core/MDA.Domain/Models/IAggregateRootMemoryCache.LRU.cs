using MDA.Infrastructure.DataStructures;
using MDA.Infrastructure.Hashes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace MDA.Domain.Models
{
    public class LruAggregateRootMemoryCache : IAggregateRootMemoryCache
    {
        private const string CacheKeyMask = "{0}.{1}";

        private readonly ILogger _logger;
        private readonly LruCache<int, IEventSourcedAggregateRoot> _cache;

        public LruAggregateRootMemoryCache(
            IOptions<AggregateRootCacheOptions> options,
            ILogger<LruAggregateRootMemoryCache> logger)
        {
            _logger = logger;

            var ops = options.Value;
            _cache = new LruCache<int, IEventSourcedAggregateRoot>(ops.MaxSize, TimeSpan.FromSeconds(ops.TTL), TimeSpan.FromSeconds(ops.MaxAge));
            _cache.RaiseFlushEvent += OnFlushed;// 单例模式下，只注册一次。
        }

        public IEventSourcedAggregateRoot Get(string aggregateRootId, Type aggregateRootType)
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
                _logger.LogError($"Getting aggregate root from memory cache[LRU] has a error, reason: The {aggregateRootType.FullName} cannot assign to {nameof(IEventSourcedAggregateRoot)}.");

                return null;
            }

            var cacheKey = GetCacheKey(aggregateRootId, aggregateRootType);

            if (_cache.TryGetValue(cacheKey, out var aggregateRoot))
            {
                var targetType = aggregateRoot.GetType();
                if (aggregateRootType != targetType)
                {
                    _logger.LogError($"Getting aggregate root from memory cache[LRU] has a error, reason: The {aggregateRootType.FullName} cannot equal to {targetType.FullName}.");

                    return null;
                }

                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug($"Got aggregate root from memory cache[LRU], Id: {aggregateRootId}, Type: {aggregateRootType}.");
                }

                return aggregateRoot;
            }

            return null;
        }

        public void Set(IEventSourcedAggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
            {
                return;
            }

            var aggregateRootId = aggregateRoot.Id;
            var aggregateRootType = aggregateRoot.GetType();
            var cacheKey = GetCacheKey(aggregateRootId, aggregateRootType);

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug($"Setting aggregate root to memory cache[LRU], Id: {aggregateRootId}, Type: {aggregateRootType}.");
            }

            _cache.Add(cacheKey, aggregateRoot);
        }

        public void Refresh(string aggregateRootId, Type aggregateRootType) => Get(aggregateRootId, aggregateRootType);

        public void Clear() => _cache.Clear();

        private int GetCacheKey(string aggregateRootId, Type aggregateRootType)
        {
            var fullName = string.Format(CacheKeyMask, aggregateRootType.FullName, aggregateRootId);

            return MurMurHash3.Hash(fullName);
        }

        private void OnFlushed(object sender, LruCache<int, IEventSourcedAggregateRoot>.FlushEventArgs args)
        {
            var aggregateRoot = args.Value;

            _logger.LogWarning($"The aggregate root over the maximum age, removed from memory LRU cache, id: {aggregateRoot.Id}, Type: {aggregateRoot.GetType().FullName}, Generation: {aggregateRoot.Generation}, Version: {aggregateRoot.Version}.");
        }
    }
}
