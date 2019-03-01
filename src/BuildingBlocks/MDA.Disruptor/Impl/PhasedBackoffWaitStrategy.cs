using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Phased wait strategy for waiting <see cref="IEventProcessor"/>s on a barrier.
    /// </summary>
    /// <remarks>
    /// This strategy can be used when throughput and low-latency are not as important as CPU resource. Spins, then yields, then waits using the configured fallback WaitStrategy.
    /// </remarks>
    public class PhasedBackoffWaitStrategy : IWaitStrategy
    {
        private const int SpinTries = 10000;
        private readonly long _spinTimeoutTicks;
        private readonly long _yieldTimeoutTicks;
        private readonly IWaitStrategy _fallbackStrategy;

        public PhasedBackoffWaitStrategy(
            TimeSpan spinTimeout,
            TimeSpan yieldTimeout,
            IWaitStrategy fallbackStrategy)
        {
            _spinTimeoutTicks = spinTimeout.Ticks;
            _yieldTimeoutTicks = spinTimeout.Ticks + yieldTimeout.Ticks;
            _fallbackStrategy = fallbackStrategy;
        }

        /// <summary>
        /// Construct <see cref="PhasedBackoffWaitStrategy"/> with fallback to <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="spinTimeout">The maximum time in to busy spin for.</param>
        /// <param name="yieldTimeout">The maximum time in to yield for.</param>
        /// <returns>The constructed wait strategy.</returns>
        public static PhasedBackoffWaitStrategy WithLock(
            TimeSpan spinTimeout,
            TimeSpan yieldTimeout)
        {
            return new PhasedBackoffWaitStrategy(spinTimeout, yieldTimeout, new BlockingWaitStrategy());
        }

        /// <summary>
        /// Construct <see cref="PhasedBackoffWaitStrategy"/> with fallback to <see cref="LiteBlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="spinTimeout">The maximum time in to busy spin for.</param>
        /// <param name="yieldTimeout">The maximum time in to yield for.</param>
        /// <returns>The constructed wait strategy.</returns>
        public static PhasedBackoffWaitStrategy WithLiteLock(
            TimeSpan spinTimeout,
            TimeSpan yieldTimeout)
        {
            return new PhasedBackoffWaitStrategy(spinTimeout, yieldTimeout, new LiteBlockingWaitStrategy());
        }

        /// <summary>
        /// Construct <see cref="PhasedBackoffWaitStrategy"/> with fallback to <see cref="SleepingWaitStrategy"/>.
        /// </summary>
        /// <param name="spinTimeout">The maximum time in to busy spin for.</param>
        /// <param name="yieldTimeout">The maximum time in to yield for.</param>
        /// <returns>The constructed wait strategy.</returns>
        public static PhasedBackoffWaitStrategy WithSleep(
            TimeSpan spinTimeout,
            TimeSpan yieldTimeout)
        {
            return new PhasedBackoffWaitStrategy(spinTimeout, yieldTimeout, new SleepingWaitStrategy());
        }

        public void SignalAllWhenBlocking()
        {
            _fallbackStrategy.SignalAllWhenBlocking();
        }

        public long WaitFor(
            long sequence, 
            ISequence cursor, 
            ISequence dependentSequence, 
            ISequenceBarrier barrier)
        {
            long startTime = 0;
            int counter = SpinTries;

            do
            {
                long availableSequence;
                if ((availableSequence = dependentSequence.GetValue()) >= sequence)
                {
                    return availableSequence;
                }

                if (0 == --counter)
                {
                    if (0 == startTime)
                    {
                        startTime = DateTime.Now.Ticks;
                    }
                    else
                    {
                        long timeDelta = DateTime.Now.Ticks - startTime;
                        if (timeDelta > _yieldTimeoutTicks)
                        {
                            return _fallbackStrategy.WaitFor(sequence, cursor, dependentSequence, barrier);
                        }
                        else if (timeDelta > _spinTimeoutTicks)
                        {
                            Thread.Yield();
                        }
                    }

                    counter = SpinTries;
                }
            } while (true);
        }
    }
}
