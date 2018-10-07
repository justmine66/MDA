using MDA.Disruptor;
using System;

namespace MDA.Test.Disruptor
{
    public class LongEventHandler : IEventHandler<LongEvent>
    {
        public void OnEvent(LongEvent @event, long sequence, bool endOfBatch)
        {
            Console.WriteLine($"{GetType().FullName}[sequence:{sequence}, endOfBatch:{endOfBatch}, event:{@event}]");
        }
    }
}
