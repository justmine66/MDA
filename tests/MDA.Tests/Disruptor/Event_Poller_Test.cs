using MDA.Disruptor;
using MDA.Disruptor.Impl;
using MDA.Disruptor.Utility;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Event_Poller_Test
    {
        [Fact(DisplayName = "轮询处理所有事件")]
        public void Should_Poll_For_Events()
        {
            var gatingSequence = new Sequence();
            var sequencer = new SingleProducerSequencer(16, new BusySpinWaitStrategy());
            var handler = new EventPollerHandler();

            var data = new object[16];
            var provider = new DataProvider(data);
            var poller = sequencer.NewPoller(provider, gatingSequence);
            var @event = new object();
            data[0] = @event;

            Assert.Equal(EventPoller<object>.PollState.Idle, poller.Poll(handler));

            sequencer.Publish(sequencer.Next());
            Assert.Equal(EventPoller<object>.PollState.Gating, poller.Poll(handler));

            gatingSequence.IncrementAndGet();
            Assert.Equal(EventPoller<object>.PollState.Processing, poller.Poll(handler));
        }

        private class EventPollerHandler : EventPoller<object>.IHandler<object>
        {
            public bool OnEvent(object @event, long sequence, bool endOfBatch)
            {
                return false;
            }
        }

        private class DataProvider : IDataProvider<object>
        {
            private readonly object[] _data;
            public DataProvider(params object[] data)
            {
                _data = data;
            }

            public object Get(long sequence)
            {
                return _data[sequence];
            }
        }
    }
}
