using System;

namespace MDA.Disruptor
{
    public abstract class AbstractSequencer : ISequencer
    {
        public AbstractSequencer(int bufferSize, IWaitStrategy waitStrategy)
        {

        }

        public int BufferSize => throw new NotImplementedException();

        public void AddGatingSequences(params ISequence[] gatingSequences)
        {
            throw new NotImplementedException();
        }

        public void Claim(long sequence)
        {
            throw new NotImplementedException();
        }

        public long GetCursor()
        {
            throw new NotImplementedException();
        }

        public long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            throw new NotImplementedException();
        }

        public long GetMinimumSequence()
        {
            throw new NotImplementedException();
        }

        public long GetRemainingCapacity()
        {
            throw new NotImplementedException();
        }

        public bool HasAvailableCapacity(int requiredCapacity)
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(long sequence)
        {
            throw new NotImplementedException();
        }

        public ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack)
        {
            throw new NotImplementedException();
        }

        public EventPoller<T> NewPoller<T>(IDataProvider<T> provider, params ISequence[] gatingSequences)
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

        public void Publish(long sequence)
        {
            throw new NotImplementedException();
        }

        public void Publish(long lo, long hi)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGatingSequence(ISequence sequence)
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
    }
}
