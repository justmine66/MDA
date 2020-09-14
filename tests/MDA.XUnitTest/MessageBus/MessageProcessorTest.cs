using MDA.MessageBus;
using Xunit;

namespace MDA.XUnitTest.MessageBus
{
    public class MessageProcessorTest
    {
        private readonly IMessagePublisher _publisher;

        public MessageProcessorTest(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        [Fact(DisplayName = "测试处理消息")]
        public void TestProcessMessage()
        {
            var message = new FakeMessage() { Payload = 1 };

            _publisher.Publish(message);
        }

        [Fact(DisplayName = "测试处理分区消息")]
        public void TestProcessMessageWithPartitionKey()
        {
            var message = new FakeMessageWithPartitionKey() { Payload = 2 };

            _publisher.Publish(message);
        }
    }
}
