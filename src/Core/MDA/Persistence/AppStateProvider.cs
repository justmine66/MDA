using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MDA.EventSourcing;
using MDA.EventStoring;

namespace MDA.Persistence
{
    public class AppStateProvider : IAppStateProvider
    {
        private readonly IEventStore _eventStore;
        private readonly 

        public AppStateProvider(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<T> GetAsync<T>(string principal) where T : EventSourcedRootEntity
        {
            var eventLog = await _eventStore.GetEventLogAsync(principal);

            var constructor = typeof(T).GetConstructor(new[] { typeof(IEnumerable<IDomainEvent>), typeof(int) });
            if (constructor == null)
                throw new InvalidOperationException($"A constructor for type '{typeof(T)}' was not found.");

            var instance = constructor.Invoke(new object[] { eventLog, 1 });

            return instance as T;
        }
    }
}
