using MDA.Disruptor.Exceptions;
using MDA.Disruptor.Infrastracture;
using MDA.Disruptor.Utility;
using System;

namespace MDA.Disruptor.Impl
{
    public abstract class SingleProducerSequencerPad : AbstractSequencer
    {
        protected long P1, P2, P3, P4, P5, P6, P7;

        protected SingleProducerSequencerPad(int bufferSize, IWaitStrategy waitStrategy)
            : base(bufferSize, waitStrategy)
        {
        }
    }

    public abstract class SingleProducerSequencerFields : SingleProducerSequencerPad
    {
        protected SingleProducerSequencerFields(int bufferSize, IWaitStrategy waitStrategy) 
            : base(bufferSize, waitStrategy)
        {
        }

        protected long NextValue = Sequence.InitialValue;
        protected long CachedValue = Sequence.InitialValue;
    }

    /// <summary>
    /// Coordinator for claiming sequences for access to a data structure while tracking dependent <see cref="ISequence"/>s. Not safe for use from multiple threads as it does not implement any barriers.
    /// </summary>
    /// <remarks>
    /// Note on <see cref="ISequencer.GetCursor()"/>: With this sequencer the cursor value is updated after the call to <see cref="ISequencer.Publish(long)"/> is made.
    /// </remarks>
    public class SingleProducerSequencer : SingleProducerSequencerFields
    {
        protected long P8, P9, P10, P11, P12, P13, P14;

        /// <summary>
        /// Construct a Sequencer with the selected wait strategy and buffer size.
        /// </summary>
        /// <param name="bufferSize">the size of the buffer that this will sequence over.</param>
        /// <param name="waitStrategy">for those waiting on sequences.</param>
        public SingleProducerSequencer(int bufferSize, IWaitStrategy waitStrategy)
            : base(bufferSize, waitStrategy)
        {
        }

        public override void Claim(long sequence)
        {
            NextValue = sequence;
        }

        public override long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            return availableSequence;
        }

        public override long GetRemainingCapacity()
        {
            long nextValue = CachedValue;

            long consumed = SequenceGroupManager.GetMinimumSequence(GatingSequences, nextValue);
            long produced = nextValue;
            return BufferSize - (produced - consumed);
        }

        public override bool HasAvailableCapacity(int requiredCapacity)
        {
            return HasAvailableCapacity(requiredCapacity, false);
        }

        public override bool IsAvailable(long sequence)
        {
            return sequence <= Cursor.GetValue();
        }

        public override long Next()
        {
            return Next(1);
        }

        public override long Next(int n)
        {
            if (n < 1)
            {
                throw new ArgumentException($"{nameof(n)} must greater than 1.");
            }

            long nextValue = NextValue;

            long nextSequence = nextValue + n;
            long wrapPoint = nextSequence - BufferSize;
            long cachedGatingSequence = CachedValue;

            if (wrapPoint > cachedGatingSequence || cachedGatingSequence > nextValue)
            {
                Cursor.SetVolatileValue(nextValue);  // StoreLoad fence

                long minSequence;
                var spinWait = default(AggressiveSpinWait);

                while (wrapPoint > (minSequence = SequenceGroupManager.GetMinimumSequence(GatingSequences, nextValue)))
                {
                    // Use waitStrategy to spin?
                    spinWait.SpinOnce();
                }

                CachedValue = minSequence;
            }

            NextValue = nextSequence;

            return nextSequence;
        }

        public override void Publish(long sequence)
        {
            Cursor.SetValue(sequence);
            WaitStrategy.SignalAllWhenBlocking();
        }

        public override void Publish(long lo, long hi)
        {
            Publish(hi);
        }

        public override bool TryNext(out long sequence)
        {
            return TryNext(1, out sequence);
        }

        public override bool TryNext(int n, out long sequence)
        {
            if (n < 1)
            {
                throw new ArgumentException($"{nameof(n)} must greater than 1.");
            }

            if (!HasAvailableCapacity(n, true))
            {
                throw InsufficientCapacityException.Instance;
            }

            sequence = NextValue += n;
            return true;
        }

        #region [private methods]

        private bool HasAvailableCapacity(int requiredCapacity, bool doStore)
        {
            var nextValue = NextValue;
            var wrapPoint = (nextValue + requiredCapacity) - BufferSize;
            var cachedGatingSequence = CachedValue;

            if (wrapPoint > cachedGatingSequence || cachedGatingSequence > nextValue)
            {
                if (doStore)
                {
                    Cursor.SetVolatileValue(nextValue);  // StoreLoad fence
                }

                long minSequence = SequenceGroupManager.GetMinimumSequence(GatingSequences, nextValue);
                CachedValue = minSequence;

                if (wrapPoint > minSequence)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}