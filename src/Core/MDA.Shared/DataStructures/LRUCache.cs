using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MDA.Shared.DataStructures
{
    /// <summary>
    /// 最近最少使用缓存(Least Recently Use)
    /// 根据缓存清理策略命名的一种数据结构，即首先清除最近最少使用的元素，一般缓存元素都会有一个时间戳，缓存系统会选择清除时间戳离当前时间最远的元素。
    /// https://github.com/dotnet/orleans/blob/6a4cdbdfc57fbcb8204a357838f99adf7f9be542/src/Orleans.Core/Utils/LRU.cs
    /// This class implements an LRU cache of values. It keeps a bounded set of values and will flush "old" values 
    /// </summary>
    /// <typeparam name="TKey">The type of key</typeparam>
    /// <typeparam name="TValue">The type of value</typeparam>
    public class LruCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        // Delegate type for fetching the value associated with a given key.
        public delegate TValue FetchValueDelegate(TKey key);

        // The following machinery is used to notify client objects when a key and its value is being flushed from the cache.
        // The client's event handler is called after the key has been removed from the cache, but when the cache is in a consistent state so that other methods on the cache may freely
        // be invoked.
        public class FlushEventArgs : EventArgs
        {
            public FlushEventArgs(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public TKey Key { get; }

            public TValue Value { get; }
        }

        public event EventHandler<FlushEventArgs> RaiseFlushEvent;

        private long _nextGeneration;
        private long _generationToFree;
        private readonly TimeSpan _maxAge;

        // We want this to be a reference type so that we can update the values in the cache
        // without having to call AddOrUpdate, which is a nuisance
        private class TimestampedValue
        {
            public readonly DateTime WhenLoaded;
            public readonly TValue Value;
            public long Generation;

            public TimestampedValue(LruCache<TKey, TValue> lru, TValue value)
            {
                Generation = Interlocked.Increment(ref lru._nextGeneration);
                Value = value;
                WhenLoaded = DateTime.UtcNow;
            }
        }

        private readonly ConcurrentDictionary<TKey, TimestampedValue> _cache;
        private readonly FetchValueDelegate _fetcher;

        public int Count => _cache.Count;
        public int MaximumSize { get; }

        /// <summary>
        /// Creates a new LRU cache.
        /// </summary>
        /// <param name="maxSize">Maximum number of entries to allow.</param>
        /// <param name="maxAge">Maximum age of an entry.</param>
        /// <param name="fetcher"></param>
        public LruCache(int maxSize, TimeSpan maxAge, FetchValueDelegate fetcher = null)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSize), "LRU maxSize must be greater than 0");
            }

            MaximumSize = maxSize;
            _maxAge = maxAge;
            _fetcher = fetcher;
            _cache = new ConcurrentDictionary<TKey, TimestampedValue>();
        }

        public void Add(TKey key, TValue value)
        {
            AdjustSize();

            var result = new TimestampedValue(this, value);
            _cache.AddOrUpdate(key, result, (k, o) => result);
        }

        public bool ContainsKey(TKey key) => _cache.ContainsKey(key);

        public bool RemoveKey(TKey key, out TValue value)
        {
            value = default;

            if (!_cache.TryRemove(key, out var tv)) return false;

            value = tv.Value;

            return true;
        }

        public void Clear()
        {
            foreach (var pair in _cache)
            {
                var args = new FlushEventArgs(pair.Key, pair.Value.Value);

                RaiseFlushEvent?.Invoke(this, args);
            }

            _cache.Clear();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;

            if (_cache.TryGetValue(key, out var result))
            {
                result.Generation = Interlocked.Increment(ref _nextGeneration);

                var age = DateTime.UtcNow.Subtract(result.WhenLoaded);

                if (age > _maxAge)
                {
                    if (!_cache.TryRemove(key, out result)) return false;
                    if (RaiseFlushEvent == null) return false;

                    var args = new FlushEventArgs(key, result.Value);
                    RaiseFlushEvent(this, args);
                    return false;
                }

                value = result.Value;
            }
            else
            {
                return false;
            }

            return true;
        }

        public TValue Get(TKey key)
        {
            if (TryGetValue(key, out var value)) return value;
            if (_fetcher == null) return value;

            value = _fetcher(key);
            Add(key, value);

            return value;
        }

        private void AdjustSize()
        {
            while (_cache.Count >= MaximumSize)
            {
                var generationToDelete = Interlocked.Increment(ref _generationToFree);
                var entryToFree = _cache.FirstOrDefault(kvp => kvp.Value.Generation == generationToDelete);

                if (entryToFree.Key == null) continue;

                var keyToFree = entryToFree.Key;
                if (!_cache.TryRemove(keyToFree, out var old)) continue;

                if (RaiseFlushEvent == null) continue;

                var args = new FlushEventArgs(keyToFree, old.Value);
                RaiseFlushEvent(this, args);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _cache.Select(p => new KeyValuePair<TKey, TValue>(p.Key, p.Value.Value)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
