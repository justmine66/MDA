using MDA.Application.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class CreateApplicationCommand : ApplicationCommand
    {
        public long Payload { get; set; }
    }
}
