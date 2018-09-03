using System.Threading.Tasks;

namespace MDA.Event.Abstractions
{
    public interface IEventStore
    {
        Task<long> CountStoredEventsAsync();
        Task<IStoredEvent[]> GetAllStoredEventsSinceAsync(long storedEventId);
        Task<IStoredEvent[]> GetAllStoredEventsBetweenAsync(long lowStoredEventId, long highStoredEventId);
        Task<IStoredEvent> AppendAsync(IDomainEvent domainEvent);
    }
}
