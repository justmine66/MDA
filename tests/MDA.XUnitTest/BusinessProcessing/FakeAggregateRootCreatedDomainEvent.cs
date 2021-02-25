using MDA.Domain.Shared.Events;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class FakeAggregateRootCreatedDomainEvent : DomainEvent<long>
    {
        public FakeAggregateRootCreatedDomainEvent(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; set; }
    }
}
