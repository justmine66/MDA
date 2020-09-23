namespace MDA.Domain.Models
{
    public interface IAggregateRootMemoryCache
    {
        IEventSourcedAggregateRoot Get(string aggregateRootId);

        void Set(IEventSourcedAggregateRoot aggregateRoot);

        void Refresh(string aggregateRootId);

        void Clear();
    }
}
