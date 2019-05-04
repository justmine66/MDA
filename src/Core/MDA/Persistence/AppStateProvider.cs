using MDA.EventStoring;
using System;

namespace MDA.Persistence
{
    public class AppStateProvider : IAppStateProvider
    {
        private readonly IEventStore _eventStore;

        public AppStateProvider(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T Get<T>(string principal) where T : class
        {
            var eventLog = _eventStore.GetEventLogAsync(principal);
            var instance = Activator.CreateInstance(typeof(T), eventLog, 1);
            return instance as T;
        }
    }
}
