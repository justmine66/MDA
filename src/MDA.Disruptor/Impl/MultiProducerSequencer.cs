using MDA.Disruptor.Exceptions;
using MDA.Disruptor.Extensions;
using MDA.Disruptor.Infrastracture;
using MDA.Disruptor.Utility;
using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Coordinator for claiming sequences for access to a data structure while tracking dependent <see cref="ISequence"/>s. Suitable for use for sequencing across multiple publisher threads.
    /// </summary>
    /// <remarks>
    /// Note on <see cref="ISequencer.GetCursor()"/>: With this sequencer the cursor value is updated after the call to <see cref="ISequencer.Next()"/>, to determine the highest available sequence that can be read, then <see cref="ISequencer.GetHighestPublishedSequence(long,long)"/> should be used.
    /// </remarks>
    public class MultiProducerSequencer : AbstractSequencer
    {
        private readonly ISequence _gatingSequenceCache = new Sequence();

        private readonly int[] _availableBuffer;
        private readonly int _indexMask;
        private readonly int _indexShift;

        /// <summary>
        /// Construct a Sequencer with the selected wait strategy and buffer size.
        /// </summary>
        /// <param name="bufferSize">the size of the buffer that this will sequence over.</param>
        /// <param name="waitStrategy">for those waiting on sequences.</param>
        public MultiProducerSequencer(int bufferSize, IWaitStrategy waitStrategy)
            : base(bufferSize, waitStrategy)
        {
            _availableBuffer = new int[bufferSize];
            _indexMask = bufferSize - 1;
            _indexShift = bufferSize.Log2();

            InitialiseAvailableBuffer();
        }

        public override void Claim(long sequence)
        {
            Cursor.SetValue(sequence);
        }

        public override bool IsAvailable(long sequence)
        {
            int index = CalculateIndex(sequence);
            int flag = CalculateAvailabilityFlag(sequence);

            return Volatile.Read(ref _availableBuffer[index]) == flag;
        }

        public override long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            for (long sequence = nextSequence; sequence <= availableSequence; sequence++)
            {
                if (!IsAvailable(sequence))
                {
                    return sequence - 1;
                }
            }

            return availableSequence;
        }

        public override long GetRemainingCapacity()
        {
            long consumed = SequenceGroupManager.GetMinimumSequence(GatingSequences, Cursor.GetValue());
            long produced = Cursor.GetValue();
            return BufferSize - (produced - consumed);
        }

        public override long Next()
        {
            return Next(1);
        }

        public override long Next(int n)
        {
            if (n < 1)
            {
                throw new ArgumentException($"{nameof(n)} must be > 0");
            }

            long next;
            long current;
            var spinWait = default(AggressiveSpinWait);

            do
            {
                current = Cursor.GetValue();
                next = current + n;

                var wrapPoint = next - BufferSize;
                var cachedGatingSequence = _gatingSequenceCache.GetValue();

                if (wrapPoint > cachedGatingSequence || cachedGatingSequence > current)
                {
                    var gatingSequence = SequenceGroupManager.GetMinimumSequence(GatingSequences, current);

                    if (wrapPoint > gatingSequence)
                    {
                        //should we spin based on the wait strategy?
                        spinWait.SpinOnce();
                        continue;
                    }

                    _gatingSequenceCache.SetValue(gatingSequence);
                }
                else if (Cursor.CompareAndSet(current, next))
                {
                    break;
                }
            } while (true);

            return next;
        }

        public override bool TryNext(out long sequence)
        {
            return TryNext(1, out sequence);
        }

        public override bool TryNext(int n, out long sequence)
        {
            if (n < 1)
            {
                throw new ArgumentException($"{n} must be > 0");
            }

            long current;
            long next;

            do
            {
                current = Cursor.GetValue();
                next = current + n;

                if (!HasAvailableCapacity(GatingSequences, n, current))
                {
                    throw InsufficientCapacityException.Instance;
                }
            }
            while (!Cursor.CompareAndSet(current, next));

            sequence = next;
            return true;
        }

        public override void Publish(long sequence)
        {
            SetAvailable(sequence);
            WaitStrategy.SignalAllWhenBlocking();
        }

        public override void Publish(long lo, long hi)
        {
            for (long l = lo; l <= hi; l++)
            {
                SetAvailable(l);
            }
            WaitStrategy.SignalAllWhenBlocking();
        }

        public override bool HasAvailableCapacity(int requiredCapacity)
        {
            return HasAvailableCapacity(GatingSequences, requiredCapacity, Cursor.GetValue());
        }

        #region [ private methods ]

        private void InitialiseAvailableBuffer()
        {
            for (int i = _availableBuffer.Length - 1; i != 0; i--)
            {
                SetAvailableBufferValue(i, -1);
            }

            SetAvailableBufferValue(0, -1);
        }

        /// <summary>
        /// The below methods work on the availableBuffer flag.
        /// The prime reason is to avoid a shared sequence object between publisher threads.(Keeping single pointers tracking start and end would require coordination between the threads).
        /// -- Firstly we have the constraint that the delta between the cursor and minimum gating sequence will never be larger than the buffer size (the code in next/tryNext in the Sequence takes care of that).
        /// -- Given that; take the sequence value and mask off the lower portion of the sequence as the index into the buffer(indexMask). (aka modulo operator)
        /// -- The upper portion of the sequence becomes the value to check for availability. ie: it tells us how many times around the ring buffer we've been (aka division).
        /// -- Because we can't wrap without the gating sequences moving forward (i.e. the minimum gating sequence is effectively our last available position in the buffer), when we have new data and successfully claimed a slot we can simply write over the top.
        /// </summary>
        /// <param name="sequence"></param>
        private void SetAvailable(long sequence)
        {
            SetAvailableBufferValue(CalculateIndex(sequence), CalculateAvailabilityFlag(sequence));
        }
        private void SetAvailableBufferValue(int index, int flag)
        {
            _availableBuffer[index] = flag;
        }

        private bool HasAvailableCapacity(ISequence[] gatingSequences, int requiredCapacity, long cursorValue)
        {
            long wrapPoint = (cursorValue + requiredCapacity) - BufferSize;
            long cachedGatingSequence = _gatingSequenceCache.GetValue();

            if (wrapPoint > cachedGatingSequence || cachedGatingSequence > cursorValue)
            {
                long minSequence = SequenceGroupManager.GetMinimumSequence(gatingSequences, cursorValue);
                _gatingSequenceCache.SetValue(minSequence);

                if (wrapPoint > minSequence)
                {
                    return false;
                }
            }

            return true;
        }

        private int CalculateIndex(long sequence)
        {
            //算法: sequence & (array length－1) = array index
            return ((int)sequence) & _indexMask;
        }

        private int CalculateAvailabilityFlag(long sequence)
        {
            return (int)((ulong)sequence >> _indexShift);
        }

        #endregion
    }
}
