using MDA.EventSourcing;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MDA.EventStoring.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, List<EventLogEntry>> _dataContainer = new ConcurrentDictionary<string, List<EventLogEntry>>(StringComparer.Ordinal);

        public Task AppendAsync(IDomainEvent domainEvent)
        {
            var key = domainEvent.Principal;
            var entry = new EventLogEntry(domainEvent);

            if (_dataContainer.TryGetValue(key, out var eventLog))
                eventLog.Add(entry);
            else
            {
                eventLog = new List<EventLogEntry> { entry };
                _dataContainer.TryAdd(domainEvent.Principal, eventLog);
            }

            return Task.CompletedTask;
        }

        public Task<long> CountAsync()
        {
            return Task.FromResult((long)_dataContainer.Count);
        }

        public void Dispose()
        {
            _dataContainer.Clear();
        }

        public Task<IEnumerable<IDomainEvent>> GetEventLogAsync(string principal)
        {
            if (_dataContainer.TryGetValue(principal, out var eventLog))
            {
                var eventStream = eventLog.Select(it => it.Payload).ToArray();
                return Task.FromResult((IEnumerable<IDomainEvent>)eventStream);
            }
            else
                return Task.FromResult<IEnumerable<IDomainEvent>>(null);
        }

        public Task<IEnumerable<IDomainEvent>> GetEventLogBetweenAsync(string principal, long lowEventId, long highEventId)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<IDomainEvent>> GetEventLogSinceAsync(string principal, long eventId)
        {
            throw new NotImplementedException();
        }
    }
}
