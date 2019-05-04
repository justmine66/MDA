using System;

namespace MDA.EventSourcing
{
    public interface IDomainEvent
    {
        /// <summary>
        /// The aggregate root uid.
        /// </summary>
        string Principal { get; set; }
        long EventVersion { get; set; }
        DateTime OccurredOn { get; set; }
    }
}
