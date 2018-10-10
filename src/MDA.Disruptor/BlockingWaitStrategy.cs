using System;

namespace MDA.Disruptor
{
    public class BlockingWaitStrategy : IWaitStrategy
    {
        public void SignalAllWhenBlocking()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            throw new NotImplementedException();
        }
    }
}
