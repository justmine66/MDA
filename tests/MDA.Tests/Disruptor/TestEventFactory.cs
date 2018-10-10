using MDA.Disruptor;

namespace MDA.Tests.Disruptor
{
    public class TestEventFactory : IEventFactory<TestEvent>
    {
        public TestEvent NewInstance()
        {
            return new TestEvent();
        }
    }
}
