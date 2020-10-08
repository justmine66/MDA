using MDA.Domain.Events;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class FakeAggregateRootCreatedDomainEvent : DomainEvent<long, long>
    {
        public FakeAggregateRootCreatedDomainEvent(long payload)
        {
            Payload = payload;
        }
    }
}
