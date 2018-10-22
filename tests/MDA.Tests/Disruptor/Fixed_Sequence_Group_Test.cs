using MDA.Disruptor.Impl;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Fixed_Sequence_Group_Test
    {
        [Fact(DisplayName = "返回两个序列组之间最小的序号。")]
        public void Should_Return_Minimum_Of_2_Sequences()
        {
            var sequence1 = new Sequence(34);
            var sequnece2 = new Sequence(47);

            var group = new FixedSequenceGroup(new Sequence[] { sequence1, sequnece2 });

            Assert.Equal(34L, group.GetValue());
            sequence1.SetValue(35);
            Assert.Equal(35L, group.GetValue());
            sequence1.SetValue(48);
            Assert.Equal(47L, group.GetValue());
        }
    }
}
