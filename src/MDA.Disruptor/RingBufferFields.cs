using System;
using MDA.Disruptor.Extensions;

namespace MDA.Disruptor
{
    public abstract class RingBufferFields<TEvent> : LhsPadding
    {
        private static readonly int _bufferPad;
        private static readonly long _refArrayBase;
        private static readonly int _refElementShift;

        static RingBufferFields()
        {
            var scale = IntPtr.Size;

            switch (scale)
            {
                case 4:
                    _refElementShift = 2;
                    break;
                case 8:
                    _refElementShift = 3;
                    break;
                default:
                    throw new PlatformNotSupportedException("Unknown pointer size");
            }

            _bufferPad = 128 / scale;
        }

        private readonly long _indexMask;
        private readonly object[] _entries;
        protected readonly int _bufferSize;
        protected readonly ISequencer _sequencer;

        protected RingBufferFields(
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

        protected TEvent ElementAt(long sequence)
        {
            return default(TEvent);
        }
    }
}
