using MDA.Disruptor.Exceptions;
using System;
using MDA.Disruptor.Infrastracture;
using MDA.Disruptor.Utility;

namespace MDA.Disruptor.Impl
{
    public abstract class SingleProducerSequencerPad : AbstractSequencer
    {
        protected long p1, p2, p3, p4, p5, p6, p7;

        public SingleProducerSequencerPad(int bufferSize, IWaitStrategy waitStrategy)
            : base(bufferSize, waitStrategy)
        {
        }
    }

    public abstract class SingleProducerSequencerFields : SingleProducerSequencerPad
    {
        public SingleProducerSequencerFields(int bufferSize, IWaitStrategy waitStrategy) : base(bufferSize, waitStrategy)
        {
        }

        protected long nextValue = Sequence.InitialValue;
        protected long cachedValue = Sequence.InitialValue;
    }

    public class SingleProducerSequencer : SingleProducerSequencerFields
    {
        protected long p8, p9, p10, p11, p12, p14, p15;

        public SingleProducerSequencer(int bufferSize, IWaitStrategy waitStrategy)
            : base(bufferSize, waitStrategy)
        {
        }

        public override void Claim(long sequence)
        {
            this.nextValue = sequence;
        }

        public override long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            return availableSequence;
        }

        public override long GetRemainingCapacity()
        {
            long nextValue = this.nextValue;

            long consumed = SequenceGroupManager.GetMinimumSequence(gatingSequences, nextValue);
            long produced = nextValue;
            return BufferSize - (produced - consumed);
        }

        public override bool HasAvailableCapacity(int requiredCapacity)
        {
            return HasAvailableCapacity(requiredCapacity, false);
        }

        public override bool IsAvailable(long sequence)
        {
            return sequence <= cursor.GetValue();
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

            long nextValue = this.nextValue;

            long nextSequence = nextValue + n;
            long wrapPoint = nextSequence - bufferSize;
            long cachedGatingSequence = this.cachedValue;

            if (wrapPoint > cachedGatingSequence || cachedGatingSequence > nextValue)
            {
                cursor.SetVolatileValue(nextValue);  // StoreLoad fence

                long minSequence;
                var spinWait = default(AggressiveSpinWait);
                while (wrapPoint > (minSequence = SequenceGroupManager.GetMinimumSequence(gatingSequences, nextValue)))
                {
                    // Use waitStrategy to spin?
                    spinWait.SpinOnce();
                }

                this.cachedValue = minSequence;
            }

            this.nextValue = nextSequence;

            return nextSequence;
        }

        public override void Publish(long sequence)
        {
            cursor.SetValue(sequence);
            waitStrategy.SignalAllWhenBlocking();
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

            sequence = this.nextValue += n;
            return true;
        }

        #region [private methods]

        private bool HasAvailableCapacity(int requiredCapacity, bool doStore)
        {
            var nextValue = this.nextValue;
            var wrapPoint = (nextValue + requiredCapacity) - bufferSize;
            var cachedGatingSequence = this.cachedValue;

            if (wrapPoint > cachedGatingSequence || cachedGatingSequence > nextValue)
            {
                if (doStore)
                {
                    cursor.SetVolatileValue(nextValue);  // StoreLoad fence
                }

                long minSequence = SequenceGroupManager.GetMinimumSequence(gatingSequences, nextValue);
                this.cachedValue = minSequence;

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
