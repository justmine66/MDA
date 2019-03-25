using Disruptor;
using MDA.Eventing;

namespace MDA.Concurrent
{
    public class InboundEventFactory : IEventFactory<InboundEvent>
    {
        public InboundEvent NewInstance()
        {
            return new InboundEvent();
        }
    }
}
