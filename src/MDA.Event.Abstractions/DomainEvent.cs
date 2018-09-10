using System;

namespace MDA.Event.Abstractions
{
    public abstract class DomainEvent : IDomainEvent
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
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTime.Now;
        }

        public string CommandId { get; set; }
        public string AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
        public int Sequence { get; set; }
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public abstract class DomainEvent<TAggregateRootIdType> : IDomainEvent<TAggregateRootIdType>
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
            Id = Guid.NewGuid().ToString("N");
            Timestamp = DateTime.Now;
        }

        public string CommandId { get; set; }
        public TAggregateRootIdType AggregateRootId { get; set; }
        public string AggregateRootTypeName { get; set; }
        public int Sequence { get; set; }
        public string Id { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
