namespace MDA.Disruptor
{
    /// <summary>
    /// <p>Concurrent sequence class used for tracking the progress of the ring buffer and event processors.Support a number of concurrent operations including CAS and order writes.<p>Also attempts to be more efficient with regards to false sharing by adding padding around the volatile field.
    /// </summary>
    public interface ISequence
    {
        /// <summary>
        /// The current value of the sequence.
        /// </summary>
        /// <remarks>Perform a volatile read of this sequence's value.</remarks>
        long GetValue();

        /// <summary>
        /// Perform an ordered write of this sequence. The intent is a Store/Store barrier between this write and any previous store.
        /// </summary>
        /// <param name="value">The new value for the sequence.</param>
        void SetValue(long value);

        /// <summary>
        /// Performs a volatile write of this sequence. The intent is a Store/Store barrier between this write and any previous write and a Store/Load barrier between this write and any subsequent volatile read.
        /// </summary>
        /// <param name="value">The new value for the sequence.</param>
        void SetVolatileValue(long value);

        /// <summary>
        /// Perform a compare and set operation on the sequence.
        /// </summary>
        /// <param name="expectedValue">The expected current value.</param>
        /// <param name="newValue">The value to update to.</param>
        /// <returns>true if the operation succeeds, false otherwise.</returns>
        bool CompareAndSet(long expectedValue, long newValue);

        /// <summary>
        /// Atomically increment the sequence by one.
        /// </summary>
        /// <returns>The value after the increment</returns>
        long IncrementAndGet();

        /// <summary>
        /// Atomically add the supplied value.
        /// </summary>
        /// <param name="increment">The value to add to the sequence.</param>
        /// <returns>The value after the increment.</returns>
        long AddAndGet(long increment);
    }
}