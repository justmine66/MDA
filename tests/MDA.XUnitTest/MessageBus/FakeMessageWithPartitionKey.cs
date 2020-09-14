using MDA.MessageBus;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessageWithPartitionKey : Message<long>
    {
        public FakeMessageWithPartitionKey()
        {
            PartitionKey = 1;
        }
    }
}
