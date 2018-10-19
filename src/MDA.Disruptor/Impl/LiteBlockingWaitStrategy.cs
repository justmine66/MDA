using System.Threading;
using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Variation of the <see cref="BlockingWaitStrategy"/> that attempts to elide conditional wake-ups when the lock is uncontended. Shows performance improvements on microbenchmarks. However this wait strategy should be considered experimental as I have not full proved the correctness of the lock elision code.
    /// </summary>
    public class LiteBlockingWaitStrategy : IWaitStrategy
    {
        private readonly object _mutex = new object();
        private int _signalNeeded = 0;

        public void SignalAllWhenBlocking()
        {
            if (Interlocked.Exchange(ref _signalNeeded, 0) == 1)
            {
                lock (_mutex)
                {
                    Monitor.PulseAll(_mutex);
                }
            }
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            long availableSequence;
            if (cursor.GetValue() < sequence)
            {
                lock (_mutex)
                {
                    do
                    {
                        Interlocked.Exchange(ref _signalNeeded, 1);

                        if (cursor.GetValue() >= sequence)
                        {
                            break;
                        }

                        barrier.CheckAlert();
                        Monitor.Wait(_mutex);
                    } while (cursor.GetValue() < sequence);
                }
            }

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
