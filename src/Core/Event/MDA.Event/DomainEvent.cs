using MDA.MessageBus;

namespace MDA.Event
{
    public abstract class DomainEvent : SequenceMessage
    {
        protected DomainEvent() { }

        protected DomainEvent(
            string commandId, 
            string aggregateRootId, 
            string aggregateRootTypeName,
            int eventSequence)
        {
            CommandId = commandId;
            AggregateRootId = aggregateRootId;
            AggregateRootTypeName = aggregateRootTypeName;
            Sequence = eventSequence;
        }

        public string CommandId { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
    }

    public abstract class DomainEvent<TAggregateRootIdType> : SequenceMessage
    {
        protected DomainEvent() { }
        protected DomainEvent(
            string commandId,
            TAggregateRootIdType aggregateRootId,
            string aggregateRootTypeName,
            int eventSequence)
        {
            CommandId = commandId;
            AggregateRootId = aggregateRootId;
            AggregateRootTypeName = aggregateRootTypeName;
            Sequence = eventSequence;
        }

        public string CommandId { get; set; }
        public TAggregateRootIdType AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
    }
}
