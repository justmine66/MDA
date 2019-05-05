using MDA.EventSourcing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.EventStoring
{
    public interface IEventStore : IDisposable
    {
        Task<long> CountAsync();
        Task AppendAsync(IDomainEvent domainEvent);
        Task<IEnumerable<IDomainEvent>> GetEventLogAsync(string principal);
        Task<IEnumerable<IDomainEvent>> GetEventLogSinceAsync(string principal, long eventId);
        Task<IEnumerable<IDomainEvent>> GetEventLogBetweenAsync(string principal, long lowEventId, long highEventId);
    }
}
