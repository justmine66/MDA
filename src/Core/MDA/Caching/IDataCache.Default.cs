using System;
using System.Collections.Concurrent;

namespace MDA.Caching
{
    public class DataCacheImpl<TSource> : IDataCache<TSource>
    {
        private readonly ConcurrentDictionary<string, Lazy<TSource>> _dataContainer = new ConcurrentDictionary<string, Lazy<TSource>>(StringComparer.Ordinal);

        public TSource Get(string name) => _dataContainer.TryGetValue(name, out var value) ? value.Value : default(TSource);

        public TSource GetOrAdd(string name, Func<TSource> createSource) =>
            _dataContainer.GetOrAdd(name, new Lazy<TSource>(createSource)).Value;

        public bool TryAdd(string name, TSource source) => _dataContainer.TryAdd(name, new Lazy<TSource>(() => source));

        public bool TryRemove(string name) => _dataContainer.TryRemove(name, out var ignored);

        public void Clear() => _dataContainer.Clear();
    }
}
