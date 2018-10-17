using MDA.Disruptor.Exceptions;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Variation of the <see cref="TimeoutBlockingWaitStrategy"/> that attempts to elide conditional wake-ups when the lock is uncontended.
    /// </summary>
    public class LiteTimeoutBlockingWaitStrategy : IWaitStrategy
    {
        private readonly object _mutex = new object();
        private int _signalNeeded = 0;
        private int timeoutInNanos;

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
                        if (!Monitor.Wait(_mutex, timeoutInNanos))
                        {
                            throw TimeoutException.Instance;
                        }
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
