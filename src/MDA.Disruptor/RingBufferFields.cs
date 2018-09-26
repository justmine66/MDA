using System;
using MDA.Disruptor.Extensions;

namespace MDA.Disruptor
{
    public class RingBufferFields<TEvent> : LhsPadding
    {
        private static readonly int _bufferPad;
        private static readonly long REF_ARRAY_BASE;
        private static readonly int REF_ELEMENT_SHIFT;

        static RingBufferFields()
        {
            var scale = IntPtr.Size;

            switch (scale)
            {
                case 4:
                    REF_ELEMENT_SHIFT = 2;
                    break;
                case 8:
                    REF_ELEMENT_SHIFT = 3;
                    break;
                default:
                    throw new PlatformNotSupportedException("Unknown pointer size");
            }

            _bufferPad = 128 / scale;
        }

        private readonly long _indexMask;
        private readonly Object[] _entries;
        protected readonly int _bufferSize;
        protected readonly ISequencer _sequencer;

        public RingBufferFields(
            ISequencer sequencer,
            IEventFactory<TEvent> eventFactory)
        {
            _sequencer = sequencer;
            _bufferSize = _sequencer.BufferSize;

            if (_bufferSize < 1)
            {
                throw new ArgumentException("bufferSize must not be less than 1");
            }

            if (_bufferSize.BitCount() != 1)
            {
                throw new ArgumentException("bufferSize must be a power of 2");
            }

            _indexMask = _bufferSize - 1;
            _entries = new object[sequencer.BufferSize + 2 * _bufferPad];
            Fill(eventFactory);
        }

        private void Fill(IEventFactory<TEvent> eventFactory)
        {
            for (int i = 0; i < _bufferSize; i++)
            {
                _entries[_bufferPad + i] = eventFactory.NewInstance();
            }
        }
    }
}
