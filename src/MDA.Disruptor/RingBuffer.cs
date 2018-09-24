using System;

namespace MDA.Disruptor
{
    public abstract class RingBufferPad
    {
        protected long p1, p2, p3, p4, p5, p6, p7;
    }

    public class RingBufferFields<E> : RingBufferPad
    {
        private static readonly int BUFFER_PAD;
        private static readonly long REF_ARRAY_BASE;
        private static readonly int REF_ELEMENT_SHIFT;

        static RingBufferFields()
        {
            var scale = IntPtr.Size;

            if (4 == scale)
            {
                REF_ELEMENT_SHIFT = 2;
            }
            else if (8 == scale)
            {
                REF_ELEMENT_SHIFT = 3;
            }
            else
            {
                throw new PlatformNotSupportedException("Unknown pointer size");
            }

            BUFFER_PAD = 128 / scale;

        }

        private readonly long indexMask;
        private readonly Object[] entries;
        protected readonly int bufferSize;
        protected readonly ISequencer sequencer;


    }

    public sealed class RingBuffer<T> { }
}
