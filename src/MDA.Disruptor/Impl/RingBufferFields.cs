using System;
using MDA.Disruptor.Extensions;

namespace MDA.Disruptor.Impl
{
    public abstract class RingBufferPad
    {
        protected long P1, P2, P3, P4, P5, P6, P7;
    }

    public abstract class RingBufferFields<TEvent> : RingBufferPad
    {
        private static readonly int BufferPad = 128 / IntPtr.Size;

        private readonly long _indexMask;
        private readonly TEvent[] _entries;

        protected int BufferSize;
        protected ISequencer Sequencer;

        protected RingBufferFields(
            ISequencer sequencer,
            IEventFactory<TEvent> eventFactory)
        {
            Sequencer = sequencer;
            BufferSize = Sequencer.GetBufferSize();

            if (BufferSize < 1)
            {
                throw new ArgumentException($"{nameof(BufferSize)} must not be less than 1");
            }

            if (BufferSize.IsNotPowerOf2())
            {
                throw new ArgumentException($"{nameof(BufferSize)} must be a power of 2");
            }

            _indexMask = BufferSize - 1;
            _entries = new TEvent[sequencer.GetBufferSize() + 2 * BufferPad];

            Fill(eventFactory);
        }

        protected TEvent ElementAt(long sequence)
        {
            return _entries[BufferPad + (int)(sequence & _indexMask)];
        }

        private void Fill(IEventFactory<TEvent> eventFactory)
        {
            for (int i = 0; i < BufferSize; i++)
            {
                _entries[BufferPad + i] = eventFactory.NewInstance();
            }
        }
    }
}