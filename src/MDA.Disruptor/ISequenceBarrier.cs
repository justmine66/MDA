namespace MDA.Disruptor
{
    using Exceptions;

    /// <summary>
    /// Coordination barrier for tracking the cursor for publishers and sequence of dependent <see cref="IEventProcessor"/>s for processing a data structure.
    /// </summary>
    public interface ISequenceBarrier
    {
        /// <summary>
        /// Wait for the given sequence to be available for consumption.
        /// </summary>
        /// <param name="sequence">sequence to wait for</param>
        /// <exception cref="AlertException">if a status change has occurred for the Disruptor.</exception>
        /// <exception cref="TimeoutException">if a timeout occurs while waiting for the supplied sequence.</exception>
        /// <exception cref="InterruptedException">if the thread needs awaking on a condition variable.</exception>
        /// <returns>the sequence up to which is available.</returns>
        long WaitFor(long sequence);

        /// <summary>
        /// Get the current cursor value that can be read.
        /// </summary>
        /// <returns>value of the cursor for entries that have been published.</returns>
        long GetCursor();

        /// <summary>
        /// The current alert status for the barrier.
        /// true if in alert otherwise false.
        /// </summary>
        bool IsAlerted { get; }

        /// <summary>
        /// Alert the <see cref="IEventProcessor"/> of a status change and stay in this status until cleared.
        /// </summary>
        void Alert();

        /// <summary>
        /// Clear the current alert status.
        /// </summary>
        void ClearAlert();

        /// <summary>
        /// Check if an alert has been raised and throw an <see cref="AlertException"/> if it has.
        /// </summary>
        /// <exception cref="AlertException">if alert has been raised.</exception>
        void CheckAlert();
    }
}