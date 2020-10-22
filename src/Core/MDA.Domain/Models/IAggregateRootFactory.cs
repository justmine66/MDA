using System;

namespace MDA.Domain.Models
{
    public interface IAggregateRootFactory
    {
        IEventSourcedAggregateRoot<TAggregateRootId> CreateAggregateRoot<TAggregateRootId>(TAggregateRootId aggregateRootId, Type aggregateRootType);
    }
}
