using MDA.EventSourcing;
using MDA.Shared;
using System;

namespace MDA.EventStoring
{
    public class EventLogEntry : IEquatable<EventLogEntry>
    {
        public EventLogEntry(IDomainEvent e)
        {
            Assert.NotNull(nameof(e), e);
            Assert.NotNullOrEmpty(nameof(e.Principal), e.Principal);

            Header = new EventLogEntryHeader()
            {
                Principal = e.Principal,
                occurredOn = e.OccurredOn,
                EventId = e.EventVersion,
                EventTypeName = e.GetType().FullName
            };

            Payload = e;
        }

        public EventLogEntryHeader Header { get; private set; }
        public IDomainEvent Payload { get; private set; }

        public bool Equals(EventLogEntry other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return Header.EventId.Equals(other.Header.EventId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EventLogEntry);
        }

        public override int GetHashCode()
        {
            return Header.EventId.GetHashCode();
        }
    }

    public class EventLogEntryHeader
    {
        /// <summary>
        /// 表示该领域事件所属业务主体(聚合根).
        /// </summary>
        public string Principal { get; set; }
        public long EventId { get; set; }
        public string EventTypeName { get; set; }
        public DateTime occurredOn { get; set; }
    }
}
