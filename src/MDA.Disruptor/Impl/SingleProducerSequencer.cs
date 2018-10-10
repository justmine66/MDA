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

        long nextValue = Sequence.InitialValue;
        long cachedValue = Sequence.InitialValue;
    }

    public class SingleProducerSequencer : SingleProducerSequencerFields
    {
        public SingleProducerSequencer(int bufferSize, IWaitStrategy waitStrategy) : base(bufferSize, waitStrategy)
        {
        }
    }
}
