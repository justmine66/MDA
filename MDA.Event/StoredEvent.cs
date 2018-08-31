using MDA.Common;
using MDA.Event.Abstractions;
using System;

namespace MDA.Event
{
    public class StoredEvent : IStoredEvent
    {
        public StoredEvent(
            string typeName,
            DateTime occurredOn,
            string eventBody,
            long eventId = -1L)
        {
            Assert.NotNullOrEmpty(typeName, nameof(typeName));
            Assert.CharacterLengthLessThan(nameof(typeName), typeName, 100);

            Assert.NotNullOrEmpty(eventBody, nameof(eventBody));
            Assert.CharacterLengthLessThan(nameof(eventBody),eventBody, 65000);

            TypeName = typeName;
            OccurredOn = occurredOn;
            EventBody = eventBody;
            EventId = eventId;
        }

        public string TypeName { get; private set; }
        public DateTime OccurredOn { get; private set; }
        public string EventBody { get; private set; }
        public long EventId { get; private set; }

        public bool Equals(IStoredEvent other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;

            return EventId.Equals(other.EventId);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StoredEvent);
        }

        public override int GetHashCode()
        {
            return EventId.GetHashCode();
        }
    }
}
