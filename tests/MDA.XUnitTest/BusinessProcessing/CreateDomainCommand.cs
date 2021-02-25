using MDA.Domain.Shared.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class CreateDomainCommand : DomainCommand<FakeAggregateRoot, long>
    {
        public CreateDomainCommand(long payload)
        {
            Payload = payload;
        }

        public long Payload { get; private set; }
    }
}
