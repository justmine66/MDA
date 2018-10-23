using MDA.Disruptor;
using MDA.Disruptor.Impl;
using MDA.Disruptor.Utility;
using System.Collections;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Event_Poller_Test
    {
        [Fact(DisplayName = "测试EventPoller的轮询状态。")]
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

        [Fact(DisplayName = "成功轮询处理RingBuffer里的所有事件。")]
        public void Should_Successfully_Poll_When_Buffer_Is_Full()
        {
            var events = new ArrayList();

            var ringBuffer = RingBuffer<byte[]>.CreateMultiProducer(() => new byte[1], 4, new SleepingWaitStrategy());

            var poller = ringBuffer.NewPoller();
            ringBuffer.AddGatingSequences(poller.GetSequence());

            var count = 4;

            for (byte i = 1; i <= count; ++i)
            {
                var next = ringBuffer.Next();
                ringBuffer.Get(next)[0] = i;
                ringBuffer.Publish(next);
            }

            // think of another thread
            poller.Poll(new DummyEventPollerHandler(events));

            Assert.Equal(4, events.Count);
        }

        private class EventPollerHandler : EventPoller<object>.IHandler<object>
        {
            public bool OnEvent(object @event, long sequence, bool endOfBatch)
            {
                return false;
            }
        }

        private class DummyEventPollerHandler : EventPoller<byte[]>.IHandler<byte[]>
        {
            private readonly ArrayList _events;

            public DummyEventPollerHandler(ArrayList events)
            {
                _events = events;
            }

            public bool OnEvent(byte[] @event, long sequence, bool endOfBatch)
            {
                _events.Add(@event);
                return !endOfBatch;
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
