using System.Threading;
using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Blocking strategy that uses a lock and condition variable for <see cref="IEventProcessor"/>s waiting on a barrier.
    /// </summary>
    /// <remarks>This strategy can be used when throughput and low-latency are not as important as CPU resource.</remarks>
    public class BlockingWaitStrategy : IWaitStrategy
    {
        private readonly object mutex = new object();

        public void SignalAllWhenBlocking()
        {
            lock (mutex)
            {
                Monitor.PulseAll(mutex);
            }
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
            var spinWait = new AggressiveSpinWait();
            while ((availableSequence = dependentSequence.GetValue()) < sequence)
            {
                barrier.CheckAlert();
                spinWait.SpinOnce();
            }

            return availableSequence;
        }
    }
}
