using Microsoft.Extensions.Options;

namespace MDA.Concurrent
{
    public class DefaultDisruptorConfigureOption : ConfigureOptions<DisruptorOptions>
    {
        public DefaultDisruptorConfigureOption(int inboundRingBufferSize = 1024, int outboundRingBufferSize = 1024)
            : base(options =>
            {
                options.InboundRingBufferSize = inboundRingBufferSize;
                options.OutboundRingBufferSize = outboundRingBufferSize;
            })
        {
        }
    }
}
