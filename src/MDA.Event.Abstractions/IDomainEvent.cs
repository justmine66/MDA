using MDA.Message.Abstractions;

namespace MDA.Event.Abstractions
{
    public interface IDomainEvent : ISequenceMessage
    {
        string CommandId { get; set; }
        string AggregateRootId { get; set; }
        string AggregateRootTypeName { get; set; }
    }

    public interface IDomainEvent<TAggregateRootIdType> : ISequenceMessage
    {
        string CommandId { get; set; }
        TAggregateRootIdType AggregateRootId { get; set; }
        string AggregateRootTypeName { get; set; }
    }
}
