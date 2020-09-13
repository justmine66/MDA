using MDA.MessageBus;
using Xunit;

namespace MDA.XUnitTest.Messages
{
    public class MessageProcessorTest
    {
        private readonly IMessagePublisher _publisher;

        public MessageProcessorTest(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        [Fact]
        public void TestProcessMessage()
        {
            var message = new FakeMessage() { Payload = 1 };

            _publisher.Publish(message);
        }
    }
}
