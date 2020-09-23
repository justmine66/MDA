using MDA.Application.Commands;

namespace MDA.XUnitTest.BusinessProcessing
{
    public class BusinessProcessingUnitTest
    {
        private readonly IApplicationCommandPublisher _publisher;

        public BusinessProcessingUnitTest(IApplicationCommandPublisher publisher)
        {
            _publisher = publisher;
        }

        public void TestCreate()
        {
            _publisher.Publish(new CreateApplicationCommand { Payload = 1 });
        }
    }
}
