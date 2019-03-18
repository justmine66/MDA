namespace MDA.Concurrent
{
    public class DisruptorOptions
    {
        /// <summary>
        /// The size of the ring buffer of the inbound <see cref="Disruptor"/>, must be power of 2.
        /// </summary>
        public int InboundRingBufferSize { get; set; } = 1024;

        /// <summary>
        /// The size of the ring buffer of the outbound <see cref="Disruptor"/>, must be power of 2.
        /// </summary>
        public int OutboundRingBufferSize { get; set; } = 1024;
    }
}
