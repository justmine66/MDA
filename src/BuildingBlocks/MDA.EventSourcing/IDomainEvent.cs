using System;

namespace MDA.EventSourcing
{
    public interface IDomainEvent
    {
        int EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
    }
}
