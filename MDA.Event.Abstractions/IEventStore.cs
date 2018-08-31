namespace MDA.Event.Abstractions
{
    public  interface IEventStore
    {
        long CountStoredEvents();
        IStoredEvent[] GetAllStoredEventsSince(long storedEventId);
        IStoredEvent[] GetAllStoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        IStoredEvent Append(IDomainEvent domainEvent);
    }
}
