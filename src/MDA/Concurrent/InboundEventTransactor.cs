using Disruptor;
using MDA.Eventing;
using System;

namespace MDA.Concurrent
{
    public class InboundEventTransactor : IEventTranslator<InboundEvent>
    {
        public void TranslateTo(InboundEvent @event, long sequence)
        {
            
        }
    }
}
