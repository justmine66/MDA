using System;

namespace MDA.Disruptor.Impl
{
    public class LiteTimeoutBlockingWaitStrategy : IWaitStrategy
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
