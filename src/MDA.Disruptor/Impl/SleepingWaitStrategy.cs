using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Sleeping strategy that initially spins, then uses a Thread.yield(), and eventually sleep(<code>Thread.Sleep(0)</code>) for the minimum number of nanos the OS and JVM will allow while the <see cref="IEventProcessor"/>s are waiting on a barrier.
    /// </summary>
    /// <remarks>
    /// This strategy is a good compromise between performance and CPU resource. Latency spikes can occur after quiet periods.It will also reduce the impact on the producing thread as it will not need signal any conditional variables to wake up the event handling thread.
    /// </remarks>
    public class SleepingWaitStrategy : IWaitStrategy
    {
        private const int DefaultRetries = 200;
        private const long DefaultSleep = 100;

        private readonly int _retries;
        private readonly long _sleepTimeNs;

        public SleepingWaitStrategy()
            : this(DefaultRetries, DefaultSleep)
        {
        }

        public SleepingWaitStrategy(int retries) : this(retries, DefaultSleep)
        {
        }

        public SleepingWaitStrategy(int retries, long sleepTimeNs)
        {
            _retries = retries;
            _sleepTimeNs = sleepTimeNs;
        }

        public void SignalAllWhenBlocking()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            long availableSequence;
            int counter = _retries;

            while ((availableSequence = dependentSequence.GetValue()) < sequence)
            {
                counter = ApplyWaitMethod(barrier, counter);
            }

            return availableSequence;
        }

        private int ApplyWaitMethod(ISequenceBarrier barrier, int counter)
        {
            barrier.CheckAlert();

            if (counter > 100)
            {
                --counter;
            }
            else if (counter > 0)
            {
                --counter;
                Thread.Yield();
            }
            else
            {
                Thread.Sleep((int)_sleepTimeNs);
            }

            return counter;
        }
    }
}
