using System;

namespace MDA.Caching
{
    public interface IDataCache<TSource> 
    {
        TSource Get(string name);
        TSource GetOrAdd(string name, Func<TSource> createSource);
        bool TryAdd(string name, TSource source);
        bool TryRemove(string name);
        void Clear();
    }
}
