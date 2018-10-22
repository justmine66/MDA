using MDA.Disruptor;
using MDA.Disruptor.Impl;
using System.Threading.Tasks;
using Xunit;

namespace MDA.Tests.Disruptor.Support
{
    public class WaitStrategyTestUtil
    {
        public static void AssertWaitForWithDelayOf(int sleepTimeMillis, IWaitStrategy waitStrategy)
        {
            var sequenceUpdater = new SequenceUpdater(sleepTimeMillis, waitStrategy);
            Task.Run(() => sequenceUpdater.Run());
            sequenceUpdater.WaitForStartup();
            var cursor = new Sequence(0);
            var sequence = waitStrategy.WaitFor(0, cursor, sequenceUpdater.sequence, new DummySequenceBarrier());

            Assert.Equal(0L, sequence);
        }
    }
}
