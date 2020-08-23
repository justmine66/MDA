using MDA.Domain.Events;

namespace MDA.Tests.BusinessProcessor
{
    public class LongDomainEvent : DomainEvent
    {
        public LongDomainEvent(long value)
        {
            Value = value;
        }

        public long Value { get; private set; }
    }
}
