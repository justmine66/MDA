using MDA.Application.Commands;
using Xunit;

namespace MDA.XUnitTest.ApplicationCommands
{
    public class ApplicationCommandProcessingTest
    {
        private readonly IApplicationCommandPublisher _publisher;

        public ApplicationCommandProcessingTest(IApplicationCommandPublisher publisher)
        {
            _publisher = publisher;
        }

        [Fact(DisplayName = "测试应用命令消息")]
        public void TestProcessApplicationCommand()
        {
            var command = new CreateApplicationCommand() { Payload = 1 };

            _publisher.Publish(command);
        }
    }
}
