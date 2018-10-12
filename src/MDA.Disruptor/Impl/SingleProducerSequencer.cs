namespace MDA.Disruptor.Impl
{
    public abstract class SingleProducerSequencerPad : AbstractSequencer
    {
        protected long p1, p2, p3, p4, p5, p6, p7;

        public SingleProducerSequencerPad(
            int bufferSize,
            IWaitStrategy waitStrategy)
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
        public SingleProducerSequencer(int bufferSize, IWaitStrategy waitStrategy) : base(bufferSize, waitStrategy)
        {
        }

        public override void Claim(long sequence)
        {
            throw new System.NotImplementedException();
        }

        public override long GetHighestPublishedSequence(long nextSequence, long availableSequence)
        {
            throw new System.NotImplementedException();
        }

        public override long GetRemainingCapacity()
        {
            throw new System.NotImplementedException();
        }

        public override bool HasAvailableCapacity(int requiredCapacity)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsAvailable(long sequence)
        {
            throw new System.NotImplementedException();
        }

        public override long Next()
        {
            throw new System.NotImplementedException();
        }

        public override long Next(int n)
        {
            throw new System.NotImplementedException();
        }

        public override void Publish(long sequence)
        {
            throw new System.NotImplementedException();
        }

        public override void Publish(long lo, long hi)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryNext(out long sequence)
        {
            throw new System.NotImplementedException();
        }

        public override bool TryNext(int n, out long sequence)
        {
            throw new System.NotImplementedException();
        }
    }
}
