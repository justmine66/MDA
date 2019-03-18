using Disruptor.Dsl;
using MDA.Common;
using MDA.Eventing;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MDA.Persistence;

namespace MDA.Concurrent
{
    public class InboundDisruptorImpl<T> : IInboundDisruptor<T>
        where T : InboundEvent, new()
    {
        private readonly Disruptor<T> _disruptor;
        private readonly IJournalable _journaler;

        public InboundDisruptorImpl(IOptions<DisruptorOptions> options, IJournalable journaler)
        {
            _journaler = journaler;
            _disruptor = new Disruptor<T>(() => new T(), options.Value.InboundRingBufferSize, TaskScheduler.Default);
        }

        public Task<bool> SendAsync(T evt)
        {
            Assert.NotNull(evt, nameof(evt));

            _disruptor

            return Task.FromResult(true);
        }
    }
}
