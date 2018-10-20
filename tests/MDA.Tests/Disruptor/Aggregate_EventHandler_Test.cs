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

        [Fact(DisplayName = "调用OnStart。")]
        public void Should_Call_OnStart_InSequence()
        {
            var aggregateEventHandler = new AggregateEventHandler<int[]>(_eh1, _eh2, _eh3);

            aggregateEventHandler.OnStart();

            AssertStartCalls(1, _eh1, _eh2, _eh3);
        }

        [Fact(DisplayName = "调用OnShutdown。")]
        public void Should_Call_OnShutdown_InSequence()
        {
            var aggregateEventHandler = new AggregateEventHandler<int[]>(_eh1, _eh2, _eh3);

            aggregateEventHandler.OnShutdown();

            AssertShutdownCalls(1, _eh1, _eh2, _eh3);
        }

        [Fact(DisplayName = "AggregateEventHandler应该能处理空EventHandlers调用。")]
        public void Should_Handle_Empty_List_Of_EventHandlers()
        {
            var aggregateEventHandler = new AggregateEventHandler<int[]>();

            aggregateEventHandler.OnEvent(new int[] { 7 }, 0L, true);
            aggregateEventHandler.OnStart();
            aggregateEventHandler.OnShutdown();
        }

        private static void AssertLastEvent(int[] @event, long sequence, params DummyEventHandler<int[]>[] ehs)
        {
            foreach (var eh in ehs)
            {
                Assert.Equal(@event, eh.LastEvent);
                Assert.Equal(sequence, eh.LastSequence);
            }
        }

        private static void AssertStartCalls(int startCalls, params DummyEventHandler<int[]>[] ehs)
        {
            foreach (var eh in ehs)
            {
                Assert.Equal(eh.StartCalls, startCalls);
            }
        }

        private static void AssertShutdownCalls(int shutdownCalls, params DummyEventHandler<int[]>[] ehs)
        {
            foreach (var eh in ehs)
            {
                Assert.Equal(eh.ShutdownCalls, shutdownCalls);
            }
        }
    }
}
