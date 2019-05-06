using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MDA.EventSourcing;

namespace MDA.EventStoring.MySql
{
    public class MySqlEventStore : IEventStore
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task AppendAsync(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDomainEvent>> GetEventLogAsync(string principal)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDomainEvent>> GetEventLogSinceAsync(string principal, long eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDomainEvent>> GetEventLogBetweenAsync(string principal, long lowEventId, long highEventId)
        {
            throw new NotImplementedException();
        }
    }
}
