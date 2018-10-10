namespace MDA.Disruptor
{
    /// <summary>
    /// A <see cref="Sequence"/> group that can dynamically have <see cref="Sequence"/>s added and removed while being thread safe.
    /// </summary>
    public interface ISequenceGroup : ISequence
    {
        /// <summary>
        /// Get the size of the group.
        /// </summary>
        /// <returns></returns>
        int Size();

        /// <summary>
        /// Add a <see cref="ISequence"/> into this aggregate.  This should only be used during initialisation.Use <see cref="ISequenceGroup"/>
        /// </summary>
        /// <param name="sequence"></param>
        void Add(ISequence sequence);

        /// <summary>
        /// Remove a <see cref="ISequence"/> from this aggregate.
        /// </summary>
        /// <param name="sequence"></param>
        /// <returns></returns>
        bool Remove(ISequence sequence);

        /// <summary>
        /// Adds a sequence to the sequence group after threads have started to publish to the Disruptor.It will set the sequences to cursor value of the ringBuffer just after adding them.  This should prevent any nasty rewind/wrapping effects.
        /// </summary>
        /// <param name="cursored"></param>
        /// <param name="sequence"></param>
        void AddWhileRunning(ICursored cursored, ISequence sequence);
    }
}
