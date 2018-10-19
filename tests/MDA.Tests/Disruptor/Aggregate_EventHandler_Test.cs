using MDA.Disruptor.Impl;
using MDA.Tests.Disruptor.Support;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Aggregate_EventHandler_Test
    {
        private readonly DummyEventHandler<int[]> eh1 = new DummyEventHandler<int[]>();
        private readonly DummyEventHandler<int[]> eh2 = new DummyEventHandler<int[]>();
        private readonly DummyEventHandler<int[]> eh3 = new DummyEventHandler<int[]>();

        [Fact(DisplayName = "调用OnEvent。")]
        public void Should_Call_OnEvent_InSequence()
        {
            int[] @event = { 7 };
            var sequence = 3L;
            var endOfBatch = true;

            var aggregateEventHandler = new AggregateEventHandler<int[]>(eh1, eh2, eh3);
            aggregateEventHandler.OnEvent(@event, sequence, endOfBatch);
            AssertLastEvent(@event, sequence, eh1, eh2, eh3);
        }

        private static void AssertLastEvent(int[] @event, long sequence, params DummyEventHandler<int[]>[] ehs)
        {
            foreach (var eh in ehs)
            {
                Assert.Equal(@event, eh.LastEvent);
                Assert.Equal(sequence, eh.LastSequence);
            }
        }
    }
}
