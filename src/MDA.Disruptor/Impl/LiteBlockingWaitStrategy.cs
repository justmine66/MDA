using System;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Variation of the {@link TimeoutBlockingWaitStrategy} that attempts to elide conditional wake-ups when the lock is uncontended.
    /// </summary>
    public class LiteBlockingWaitStrategy : IWaitStrategy
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
