using MDA.Disruptor.Utility;

namespace MDA.Disruptor
{
    /// <summary>
    /// Coordinates claiming sequences for access to a data structure while tracking dependent <see cref="ISequence"/>s
    /// </summary>
    public interface ISequencer : ICursored, ISequenced
    {
        /// <summary>
        /// Claim a specific sequence. Only used if initialising the ring buffer to a specific value.
        /// </summary>
        /// <param name="sequence">The sequence to initialise too.</param>
        void Claim(long sequence);

        /// <summary>
        /// Confirms if a sequence is published and the event is available for use; non-blocking.
        /// </summary>
        /// <param name="sequence">of the buffer to check</param>
        /// <returns>true if the sequence is available for use, false if not.</returns>
        bool IsAvailable(long sequence);

        /// <summary>
        /// Add the specified gating sequences to this instance of the Disruptor.  They will safely and atomically added to the list of gating sequences.
        /// </summary>
        /// <param name="gatingSequences">gatingSequences The sequences to add.</param>
        void AddGatingSequences(params ISequence[] gatingSequences);

        /// <summary>
        /// Remove the specified sequence from this sequencer.
        /// </summary>
        /// <param name="sequence">to be removed.</param>
        /// <returns>true if this sequence was found, false otherwise.</returns>
        bool RemoveGatingSequence(ISequence sequence);

        /// <summary>
        /// Create a new SequenceBarrier to be used by an <see cref="IEventProcessor"/> to track which messages are available to be read from the ring buffer given a list of sequences to track.
        /// </summary>
        /// <param name="sequencesToTrack">All of the sequences that the newly constructed barrier will wait on.</param>
        /// <returns>A sequence barrier that will track the specified sequences.</returns>
        ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack);

        /// <summary>
        /// Get the minimum sequence value from all of the gating sequences
        /// added to this ringBuffer.
        /// </summary>
        /// <returns>The minimum gating sequence or the cursor sequence if no sequences have been added.</returns>
        long GetMinimumSequence();

        /// <summary>
        /// Get the highest sequence number that can be safely read from the ring buffer. Depending on the implementation of the Sequencer this call may need to scan a number of values in the Sequencer. The scan will range from nextSequence to availableSequence. If there are no available values <code> nextSequence <= sequence <= availableSequence</code> the return value will be <code>nextSequence - 1</code>. To work correctly a consumer should pass a value that is 1 higher than the last sequence that was successfully processed.
        /// </summary>
        /// <param name="nextSequence">The sequence to start scanning from.</param>
        /// <param name="availableSequence">The sequence to scan to.</param>
        /// <returns>The highest value that can be safely read from the ring buffer, will be at least <code>nextSequence - 1</code>.</returns>
        long GetHighestPublishedSequence(long nextSequence, long availableSequence);

        EventPoller<T> NewPoller<T>(IDataProvider<T> provider, params ISequence[] gatingSequences);
    }
}