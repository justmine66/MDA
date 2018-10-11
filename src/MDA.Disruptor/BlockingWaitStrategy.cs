using System;
using System.Threading;

namespace MDA.Disruptor
{
    /// <summary>
    /// Blocking strategy that uses a lock and condition variable for <see cref="IEventProcessor"/>s waiting on a barrier.
    /// </summary>
    /// <remarks>This strategy can be used when throughput and low-latency are not as important as CPU resource.</remarks>
    public class BlockingWaitStrategy : IWaitStrategy
    {
        private readonly Object mutex = new Object();

        public void SignalAllWhenBlocking()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            if (cursor.GetValue() < sequence)
            {
                lock(mutex)
                {
                    while (cursor.GetValue() < sequence)
                    {
                        barrier.CheckAlert();
                        Monitor.Wait(mutex);
                    }
                }
            }

            long availableSequence;
            var aggressiveSpinWait = new AggressiveSpinWait();
            while ((availableSequence = dependentSequence.GetValue()) < sequence)
            {
                barrier.CheckAlert();
                aggressiveSpinWait.SpinOnce();
            }

            return availableSequence;
        }
    }
}
