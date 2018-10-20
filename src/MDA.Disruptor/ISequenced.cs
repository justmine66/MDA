namespace MDA.Disruptor
{
    public interface ISequenced
    {
        /// <summary>
        /// The capacity of the data structure to hold entries.
        /// </summary>
        /// <returns>the size of the RingBuffer.</returns>
        int GetBufferSize();

        /// <summary>
        /// Has the buffer got capacity to allocate another sequence. This is a concurrent method so the response should only be taken as an indication of available capacity.
        /// </summary>
        /// <param name="requiredCapacity">requiredCapacity in the buffer</param>
        /// <returns>true if the buffer has the capacity to allocate the next sequence otherwise false.</returns>
        bool HasAvailableCapacity(int requiredCapacity);

        /// <summary>
        /// Get the remaining capacity for this sequencer.
        /// </summary>
        /// <returns></returns>
        long GetRemainingCapacity();

        /// <summary>
        /// Claim the next event in sequence for publishing.
        /// </summary>
        /// <returns>the claimed sequence value</returns>
        long Next();

        /// <summary>
        /// Claim the next n events in sequence for publishing. This is for batch event producing. Using batch producing requires a little care and some math.
        /// <code>
        /// int n = 10;
        /// long hi = sequencer.Next(n);
        /// long lo = hi - (n - 1);
        /// for (long sequence = lo; sequence <= hi; sequence++) {
        ///     // Do work.
        /// }
        /// sequencer.publish(lo, hi);
        /// </code>
        /// </summary>
        /// <param name="n">the number of sequences to claim</param>
        /// <returns>the highest claimed sequence value</returns>
        long Next(int n);

        /// <summary>
        /// Attempt to claim the next event in sequence for publishing.  Will return the number of the slot if there is at least<code>requiredCapacity</code> slots available.
        /// </summary>
        /// <param name="sequence">the claimed sequence value</param>
        /// <returns>true of there is space available in the ring buffer, otherwise false.</returns>
        bool TryNext(out long sequence);

        /// <summary>
        /// Attempt to claim the next <code>n</code> events in sequence for publishing.
        /// Will return the highest numbered slot if there is at least <code>n</code> slots
        /// available.
        /// 
        /// Have a look at <see cref="Next(int)"/> for a description on how to
        /// use this method.
        /// </summary>
        /// <param name="n">the number of sequences to claim</param>
        /// <param name="sequence">the claimed sequence value</param>
        /// <returns>true of there is space available in the ring buffer, otherwise false.</returns>
        bool TryNext(int n, out long sequence);

        /// <summary>
        /// Publishes a sequence. Call when the event has been filled.
        /// </summary>
        /// <param name="sequence">the sequence to be published.</param>
        void Publish(long sequence);

        /// <summary>
        /// Batch publish sequences. Called when all of the events have been filled.
        /// </summary>
        /// <param name="lo">the first sequence number to publish</param>
        /// <param name="hi">the last sequence number to publish</param>
        void Publish(long lo, long hi);
    }
}
