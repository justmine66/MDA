using System;

namespace MDA.Domain.Models
{
    public interface IAggregateRootFactory
    {
        IEventSourcedAggregateRoot CreateAggregateRoot(string aggregateRootId, Type aggregateRootType);
    }
}
