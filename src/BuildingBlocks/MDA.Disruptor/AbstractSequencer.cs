using MDA.Disruptor.Extensions;
using MDA.Disruptor.Impl;
using MDA.Disruptor.Utility;
using System;

namespace MDA.Disruptor
{
    /// <summary>
    /// Base class for the various sequencer types (single/multi). Provides common functionality like the management of gating sequences(add/remove) and ownership of the current cursor.
    /// </summary>
    public abstract class AbstractSequencer : ISequencer
    {
        protected IWaitStrategy WaitStrategy;
        protected ISequence Cursor = new Sequence();
        protected volatile ISequence[] GatingSequences = new ISequence[0];

        /// <summary>
        /// Create with the specified buffer size and wait strategy.
        /// </summary>
        /// <param name="bufferSize">The total number of entries, must be a positive power of 2.</param>
        /// <param name="waitStrategy">The wait strategy used by this sequencer.</param>
        protected AbstractSequencer(int bufferSize, IWaitStrategy waitStrategy)
        {
            if (bufferSize < 1)
            {
                throw new ArgumentException("bufferSize must not be less than 1");
            }

            if (bufferSize.IsNotPowerOf2())
            {
                throw new ArgumentException("bufferSize must be a power of 2");
            }

            BufferSize = bufferSize;
            WaitStrategy = waitStrategy;
        }

        public int BufferSize { get; }

        public virtual void AddGatingSequences(params ISequence[] gatingSequences)
        {
            var sequences = GatingSequences;
            SequenceGroupManager.AddSequences(ref sequences, this, gatingSequences);
        }

        public abstract void Claim(long sequence);

        public virtual long GetCursor()
        {
            return Cursor.GetValue();
        }

        public abstract long GetHighestPublishedSequence(long nextSequence, long availableSequence);

        public virtual long GetMinimumSequence()
        {
            return SequenceGroupManager.GetMinimumSequence(GatingSequences, Cursor.GetValue());
        }

        public abstract long GetRemainingCapacity();

        public int GetBufferSize()
        {
            return BufferSize;
        }

        public abstract bool HasAvailableCapacity(int requiredCapacity);

        public abstract bool IsAvailable(long sequence);

        public virtual ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack)
        {
            return new ProcessingSequenceBarrier(this, WaitStrategy, Cursor, sequencesToTrack);
        }

        public virtual EventPoller<T> NewPoller<T>(IDataProvider<T> provider, params ISequence[] gatingSequences)
        {
            return EventPoller<T>.NewInstance(provider, this, new Sequence(), Cursor, gatingSequences);
        }

        public abstract long Next();

        public abstract long Next(int n);

        public abstract void Publish(long sequence);

        public abstract void Publish(long lo, long hi);

        public virtual bool RemoveGatingSequence(ISequence sequence)
        {
            var sequences = GatingSequences;
            return SequenceGroupManager.RemoveSequence(ref sequences, sequence);
        }

        public abstract bool TryNext(out long sequence);

        public abstract bool TryNext(int n, out long sequence);
    }
}
