using System;
using MDA.Disruptor.Extensions;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Coordinator for claiming sequences for access to a data structure while tracking dependent <see cref="ISequence"/>s. Suitable for use for sequencing across multiple publisher threads.
    /// </summary>
    public class MultiProducerSequencer : ISequencer
    {
        private readonly ISequence _gatingSequenceCache;

        private int[] _availableBuffer;
        private int _indexMask;
        private int _indexShift;

        public MultiProducerSequencer(ISequence sequence)
        {
            _gatingSequenceCache = sequence;
        }

        public void Initialize(int bufferSize, IWaitStrategy waitStrategy)
        {
            _availableBuffer = new int[bufferSize];
            _indexMask = bufferSize - 1;
            _indexShift = bufferSize.Log2();

            InitialiseAvailableBuffer();
        }

        public long GetCursor()
        {
            throw new NotImplementedException();
        }

        public int BufferSize { get; }
        public bool HasAvailableCapacity(int requiredCapacity)
        {
            throw new NotImplementedException();
        }

        public long GetRemainingCapacity()
        {
            throw new NotImplementedException();
        }

        public long Next()
        {
            throw new NotImplementedException();
        }

        public long Next(int n)
        {
            throw new NotImplementedException();
        }

        public bool TryNext(out long sequence)
        {
            throw new NotImplementedException();
        }

        public bool TryNext(int n, out long sequence)
        {
            throw new NotImplementedException();
        }

        public void Publish(long sequence)
        {
            throw new NotImplementedException();
        }

        public void Publish(long lo, long hi)
        {
            throw new NotImplementedException();
        }

        public void Claim(long sequence)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(long sequence)
        {
            throw new NotImplementedException();
        }

        public void AddGatingSequences(params ISequence[] gatingSequences)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGatingSequence(ISequence sequence)
        {
            throw new NotImplementedException();
        }

        public ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack)
        {
            throw new NotImplementedException();
        }

        public long GetMinimumSequence()
        {
            throw new NotImplementedException();
        }

        public long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            throw new NotImplementedException();
        }

        public EventPoller<T> NewPoller<T>(IDataProvider<T> provider, params ISequence[] gatingSequences)
        {
            throw new NotImplementedException();
        }

        private void InitialiseAvailableBuffer()
        {
            for (int i = _availableBuffer.Length - 1; i != 0; i--)
            {
                SetAvailableBufferValue(i, -1);
            }

            SetAvailableBufferValue(0, -1);
        }

        private void SetAvailableBufferValue(int index, int flag)
        {
            _availableBuffer[index] = flag;
        }
    }
}
