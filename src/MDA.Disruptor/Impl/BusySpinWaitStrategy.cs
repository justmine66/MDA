using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Busy Spin strategy that uses a busy spin loop for <see cref="IEventProcessor"/>s waiting on a barrier.
    /// </summary>
    /// <remarks>
    /// This strategy will use CPU resource to avoid syscalls which can introduce latency jitter. It is best used when threads can be bound to specific CPU cores.
    /// </remarks>
    public class BusySpinWaitStrategy : IWaitStrategy
    {
        public void SignalAllWhenBlocking()
        {
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            long availableSequence;
            var spinWait = default(AggressiveSpinWait);

            while ((availableSequence = dependentSequence.GetValue()) < sequence)
            {
                barrier.CheckAlert();
                spinWait.SpinOnce();
            }

            return availableSequence;
        }
    }
}
