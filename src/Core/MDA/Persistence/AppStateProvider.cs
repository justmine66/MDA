using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MDA.Caching;
using MDA.EventSourcing;
using MDA.EventStoring;

namespace MDA.Persistence
{
    public class AppStateProvider : IAppStateProvider
    {
        private readonly IEventStore _eventStore;
        private readonly IDataCache<object> _cache;

        public AppStateProvider(IEventStore eventStore, IDataCache<object> cache)
        {
            _eventStore = eventStore;
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string principal) where T : EventSourcedRootEntity
        {
            var instance = _cache.Get(principal);
            if (instance is T result)
                return result;

            var eventLog = await _eventStore.GetEventLogAsync(principal);

            var constructor = typeof(T).GetConstructor(new[] { typeof(IEnumerable<IDomainEvent>), typeof(int) });
            if (constructor == null)
                throw new InvalidOperationException($"A constructor for type '{typeof(T)}' was not found.");

            var obj = constructor.Invoke(new object[] { eventLog, 1 }) as T;
            _cache.TryAdd(principal, obj);

            return obj;
        }
    }
}
