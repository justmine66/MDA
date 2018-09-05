using MDA.Message.Abstractions;

namespace MDA.Event.Abstractions
{
    public interface IDomainEvent : ISequenceMessage
    {
        string CommandId { get; set; }
        string AggregateRootId { get; set; }
        string AggregateRootTypeName { get; set; }

        IDomainEventStore ToStoredEvent();
    }

    public interface IDomainEvent<TType> : IDomainEvent
    {
        new TType AggregateRootId { get; set; }
    }
}
