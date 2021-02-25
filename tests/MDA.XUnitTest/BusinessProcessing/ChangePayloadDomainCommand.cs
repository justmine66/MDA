using MDA.Domain.Shared.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class ChangePayloadDomainCommand : DomainCommand<FakeAggregateRoot,long>
    {
        public ChangePayloadDomainCommand(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; set; }
    }
}
