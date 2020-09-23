using MDA.Domain.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class ChangePayloadDomainCommand : DomainCommand<long>
    {
        public ChangePayloadDomainCommand(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; set; }
    }
}
