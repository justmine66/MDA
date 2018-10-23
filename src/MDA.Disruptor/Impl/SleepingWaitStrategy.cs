using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Sleeping strategy that initially spins, then uses a Thread.yield(), and eventually sleep(<code>Thread.Sleep(0)</code>) for the minimum number of milliseconds the OS and CLR will allow while the <see cref="IEventProcessor"/>s are waiting on a barrier.
    /// </summary>
    /// <remarks>
    /// This strategy is a good compromise between performance and CPU resource. Latency spikes can occur after quiet periods. It will also reduce the impact on the producing thread as it will not need signal any conditional variables to wake up the event handling thread.
    /// </remarks>
    public class SleepingWaitStrategy : IWaitStrategy
    {
        private const int DefaultRetries = 200;
        private const int DefaultSleep = 100;

        private readonly int _retries;
        private readonly int _sleepTimeMillis;

        public SleepingWaitStrategy()
            : this(DefaultRetries, DefaultSleep)
        {
        }

        public SleepingWaitStrategy(int retries) 
            : this(retries, DefaultSleep)
        {
        }

        public SleepingWaitStrategy(int retries, int millisecondsSleepTime)
        {
            _retries = retries;
            _sleepTimeMillis = millisecondsSleepTime;
        }

        public void SignalAllWhenBlocking()
        {
            
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            long availableSequence;
            var counter = _retries;

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
                Thread.Sleep(_sleepTimeMillis);
            }

            return counter;
        }
    }
}
