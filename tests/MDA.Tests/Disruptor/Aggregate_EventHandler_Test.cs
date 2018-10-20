using MDA.Disruptor.Impl;
using MDA.Tests.Disruptor.Support;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Aggregate_EventHandler_Test
    {
        private readonly DummyEventHandler<int[]> _eh1 = new DummyEventHandler<int[]>();
        private readonly DummyEventHandler<int[]> _eh2 = new DummyEventHandler<int[]>();
        private readonly DummyEventHandler<int[]> _eh3 = new DummyEventHandler<int[]>();

        [Fact(DisplayName = "调用OnEvent。")]
        public void Should_Call_OnEvent_InSequence()
        {
            int[] @event = { 7 };
            var sequence = 3L;

            var aggregateEventHandler = new AggregateEventHandler<int[]>(_eh1, _eh2, _eh3);
            aggregateEventHandler.OnEvent(@event, sequence, true);
            AssertLastEvent(@event, sequence, _eh1, _eh2, _eh3);
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
