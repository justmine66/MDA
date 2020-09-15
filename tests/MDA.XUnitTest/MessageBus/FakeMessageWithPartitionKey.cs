using MDA.MessageBus;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessageWithPartitionKey : Message
    {
        public FakeMessageWithPartitionKey()
        {
            PartitionKey = 1;
        }

        public long Payload { get; set; }
    }
}
