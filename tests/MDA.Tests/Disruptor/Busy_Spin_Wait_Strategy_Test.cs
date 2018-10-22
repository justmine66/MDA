namespace MDA.Tests.Disruptor
{
    using MDA.Disruptor.Impl;
    using Xunit;
    using static MDA.Tests.Disruptor.Support.WaitStrategyTestUtil;

    public class Busy_Spin_Wait_Strategy_Test
    {
        [Fact(DisplayName = "应该等待")]
        public void Should_Wait_For_Value()
        {
            AssertWaitForWithDelayOf(50, new BusySpinWaitStrategy());
        }
    }
}
