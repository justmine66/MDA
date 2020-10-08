using MDA.Domain.Events;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class FakePayloadChangedDomainEvent : DomainEvent<long, long>
    {
        public FakePayloadChangedDomainEvent(long payload)
        {
            Payload = payload;
        }
    }
}
