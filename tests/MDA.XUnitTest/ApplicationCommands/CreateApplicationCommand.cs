using MDA.Application.Commands;

namespace MDA.XUnitTest.ApplicationCommands
{
    public class CreateApplicationCommand : ApplicationCommand
    {
        public long Payload { get; set; }
    }
}
