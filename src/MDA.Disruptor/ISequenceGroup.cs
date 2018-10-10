namespace MDA.Disruptor
{
    /// <summary>
    /// A <see cref="ISequence"/> group that can dynamically have <see cref="ISequence"/>s added and removed while being thread safe.
    /// </summary>
    /// <remarks>
    /// The <see cref="ISequenceGroup.GetMinimumSequence"/> and <see cref="ISequenceGroup.SetValue"/> methods are lock free and can beconcurrently be called with the <see cref="ISequenceGroup.Add"/> and <see cref="ISequenceGroup.Remove"/>}.
    /// </remarks>
    public interface ISequenceGroup
    {
        /// <summary>
        /// Get the minimum sequence value for the group.
        /// </summary>
        /// <returns>the minimum sequence value for the group.</returns>
        long GetMinimumSequence();

        /// <summary>
        /// Set all <see cref="ISequence"/>s in the group to a given value.
        /// </summary>
        /// <param name="value">The new value for the sequence group.</param>
        void SetValue(long value);

        /// <summary>
        /// Volatile set all <see cref="ISequence"/>s in the group to a given value.
        /// </summary>
        /// <param name="value">The new value for the sequence group.</param>
        void SetVolatileValue(long value);

        /// <summary>
        /// Get the size of the group.
        /// </summary>
        /// <returns>the size of the group.</returns>
        int Size();

        /// <summary>
        /// Add a <see cref="ISequence"/> into this aggregate.  This should only be used during initialisation.Use <see cref="ISequenceGroup"/>
        /// </summary>
        /// <param name="sequence">sequence to be added to the aggregate.</param>
        void Add(ISequence sequence);

        /// <summary>
        /// Remove a <see cref="ISequence"/> from this aggregate.
        /// </summary>
        /// <param name="sequence">sequence to be removed from this aggregate.</param>
        /// <returns></returns>
        bool Remove(ISequence sequence);

        /// <summary>
        /// Adds a sequence to the sequence group after threads have started to publish to the Disruptor.It will set the sequences to cursor value of the ringBuffer just after adding them.  This should prevent any nasty rewind/wrapping effects.
        /// </summary>
        /// <param name="cursored">The data structure that the owner of this sequence group will be pulling it's events from.</param>
        /// <param name="sequence">sequence The sequence to add.</param>
        void AddWhileRunning(ICursored cursored, ISequence sequence);
    }
}
