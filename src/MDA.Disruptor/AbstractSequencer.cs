using MDA.Disruptor.Extensions;
using MDA.Disruptor.Impl;
using System;

namespace MDA.Disruptor
{
    /// <summary>
    /// Base class for the various sequencer types (single/multi). Provides common functionality like the management of gating sequences(add/remove) and ownership of the current cursor.
    /// </summary>
    public abstract class AbstractSequencer : ISequencer
    {
        protected int bufferSize;
        protected IWaitStrategy waitStrategy;
        protected ISequence cursor = new Sequence();
        protected volatile ISequence[] gatingSequences = new Sequence[0];

        /// <summary>
        /// Create with the specified buffer size and wait strategy.
        /// </summary>
        /// <param name="bufferSize">The total number of entries, must be a positive power of 2.</param>
        /// <param name="waitStrategy">The wait strategy used by this sequencer</param>
        public AbstractSequencer(int bufferSize, IWaitStrategy waitStrategy)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentException("bufferSize must not be less than 1");
            }

            if (bufferSize.IsNotPowerOf2())
            {
                throw new ArgumentException("bufferSize must be a power of 2");
            }

            this.bufferSize = bufferSize;
            this.waitStrategy = waitStrategy;
        }

        public int BufferSize => this.bufferSize;

        public void AddGatingSequences(params ISequence[] gatingSequences)
        {
            SequenceGroupManager.AddSequences(ref this.gatingSequences, this, gatingSequences);
        }

        public abstract void Claim(long sequence);

        public long GetCursor()
        {
            return this.cursor.GetValue();
        }

        public abstract long GetHighestPublishedSequence(long nextSequence, long availableSequence);

        public long GetMinimumSequence()
        {
            return SequenceGroupManager.GetMinimumSequence(this.gatingSequences, cursor.GetValue());
        }

        public abstract long GetRemainingCapacity();

        public abstract bool HasAvailableCapacity(int requiredCapacity);

        public abstract bool IsAvailable(long sequence);

        public ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack)
        {
            return new ProcessingSequenceBarrier(this, waitStrategy, cursor, sequencesToTrack);
        }

        public EventPoller<T> NewPoller<T>(IDataProvider<T> provider, params ISequence[] gatingSequences)
        {
            return EventPoller<T>.NewInstance(provider, this, new Sequence(), cursor, gatingSequences);
        }

        public abstract long Next();

        public abstract long Next(int n);

        public abstract void Publish(long sequence);

        public abstract void Publish(long lo, long hi);

        public bool RemoveGatingSequence(ISequence sequence)
        {
            return SequenceGroupManager.RemoveSequence(ref this.gatingSequences, sequence);
        }

        public abstract bool TryNext(out long sequence);

        public abstract bool TryNext(int n, out long sequence);
    }
}
