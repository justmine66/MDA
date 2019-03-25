using Disruptor.Dsl;
using MDA.Common;
using MDA.Eventing;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public class InboundDisruptorImpl : IInboundDisruptor
    {
        private readonly Disruptor<InboundEvent> _disruptor;
        private readonly IOptions<DisruptorOptions> _options;

        public InboundDisruptorImpl(Disruptor<InboundEvent> disruptor, IOptions<DisruptorOptions> options)
        {
            _disruptor = disruptor;
            _options = options;
        }

        public Task<bool> SendAsync<T>(T evt) where T : InboundEvent, new()
        {
            Assert.NotNull(evt, nameof(evt));

            var ringBuffer = _disruptor.GetRingBuffer();
            var sequence = ringBuffer.Next();

            try
            {
                var next = ringBuffer.Get(sequence);
                next = evt;
            }
            finally
            {
                ringBuffer.Publish(sequence);
            }

            return Task.FromResult(true);
        }
    }
}
