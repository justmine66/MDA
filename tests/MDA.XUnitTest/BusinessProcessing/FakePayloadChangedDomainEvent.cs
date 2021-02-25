using MDA.Domain.Shared.Events;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class FakePayloadChangedDomainEvent : DomainEvent<long>
    {
        public FakePayloadChangedDomainEvent(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; set; }
    }
}
