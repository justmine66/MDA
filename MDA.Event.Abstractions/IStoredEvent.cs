using System;

namespace MDA.Event.Abstractions
{
    public interface IStoredEvent : IEquatable<IStoredEvent>
    {
        string TypeName { get; }
        DateTime OccurredOn { get; }
        string EventBody { get; }
        long EventId { get; }
    }
}
