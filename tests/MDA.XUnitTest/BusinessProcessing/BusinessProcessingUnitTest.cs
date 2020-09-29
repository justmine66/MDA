using MDA.Application.Commands;
using MDA.XUnitTest.ApplicationCommands;
using Xunit;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class BusinessProcessingUnitTest
    {
        private readonly IApplicationCommandPublisher _publisher;

        public BusinessProcessingUnitTest(IApplicationCommandPublisher publisher)
        {
            _publisher = publisher;
        }

        [Fact]
        public void TestCreate()
        {
            _publisher.Publish(new CreateApplicationCommand { Payload = 1 });
        }
    }
}
