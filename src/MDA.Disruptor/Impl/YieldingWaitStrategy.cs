using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Yielding strategy that uses a Thread.yield() for <see cref="IEventProcessor"/>s waiting on a barrier after an initially spinning.
    /// </summary>
    /// <remarks>This strategy will use 100% CPU, but will more readily give up the CPU than a busy spin strategy if other threads require CPU resource.</remarks>
    public class YieldingWaitStrategy : IWaitStrategy
    {
        private const int SpinTries = 100;

        public void SignalAllWhenBlocking()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            long availableSequence;
            int counter = SpinTries;

            while ((availableSequence = dependentSequence.GetValue()) < sequence)
            {
                counter = ApplyWaitMethod(barrier, counter);
            }

            return availableSequence;
        }

        private int ApplyWaitMethod(ISequenceBarrier barrier, int counter)
        {
            barrier.CheckAlert();

            if (0 == counter)
            {
                Thread.Yield();
            }
            else
            {
                --counter;
            }

            return counter;
        }
    }
}
