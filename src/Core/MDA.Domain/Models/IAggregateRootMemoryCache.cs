using System;

namespace MDA.Domain.Models
{
    public interface IAggregateRootMemoryCache
    {
        IEventSourcedAggregateRoot Get(string aggregateRootId, Type aggregateRootType);

        void Set(IEventSourcedAggregateRoot aggregateRoot);

        void Refresh(string aggregateRootId, Type aggregateRootType);

        void Clear();
    }
}
