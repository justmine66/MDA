using MDA.Disruptor.Exceptions;
using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// Strategy employed for making <see cref="IEventProcessor"/>s wait on a cursor <see cref="ISequence"/>.
    /// </summary>
    public interface IWaitStrategy
    {
        /// <summary>
        /// Wait for the given sequence to be available. It is possible for this method to return a value less than the sequence number supplied depending on the implementation of the WaitStrategy. A common use for this is to signal a timeout. Any EventProcessor that is using a WaitStrategy to get notifications about message becoming available should remember to handle this case. The <see cref="IBatchEventProcessor{TEvent}"/> explicitly handles this case and will signal a timeout if required.
        /// </summary>
        /// <param name="sequence">to be waited on.</param>
        /// <param name="cursor">the main sequence from <see cref="RingBuffer{TEvent}"/>. Wait/notify strategies will need this as it's the only sequence that is also notified upon update.</param>
        /// <param name="dependentSequence">on which to wait.</param>
        /// <param name="barrier">the processor is waiting on.</param>
        /// <returns>the sequence that is available which may be greater than the requested sequence.</returns>
        /// <example cref="AlertException">if the status of the Disruptor has changed.</example>
        /// <example cref="InterruptedException">if the thread is interrupted.</example>
        /// <example cref="TimeoutException">if a timeout occurs before waiting completes (not used by some strategies).</example>
        long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier);

        /// <summary>
        /// Implementations should signal the waiting <see cref="IEventProcessor"/>s that the cursor has advanced.
        /// </summary>
        void SignalAllWhenBlocking();
    }
}
