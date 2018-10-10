using System;
using MDA.Disruptor;
using MDA.Disruptor.Impl;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Sequence_Group_Test
    {
        [Fact(DisplayName = "空SequenceGroup应该返回最大的序号。")]
        public void Should_Return_Max_Sequence_When_Empty_Group()
        {
            var group = new SequenceGroup();

            Assert.Equal(long.MaxValue, group.GetMinimumSequence());
        }

        [Fact(DisplayName = "应该能够成功移除不存在的序号。")]
        public void Should_Not_Fail_If_Trying_To_Remove_NotExisting_Sequence()
        {
            var group = new SequenceGroup();
            group.Add(new Sequence());
            group.Add(new Sequence());
            group.Remove(new Sequence());
        }

        [Fact(DisplayName = "应该返回最小的序号。")]
        public void Should_Report_The_Minimum_Sequence_For_GroupOfTwo()
        {
            var sequenceThree = new Sequence(3L);
            var sequenceSeven = new Sequence(7L);

            var group = new SequenceGroup();
            group.Add(sequenceThree);
            group.Add(sequenceSeven);

            Assert.Equal(sequenceThree.GetValue(), group.GetMinimumSequence());
        }

        [Fact(DisplayName = "获取SequenceGroup的Size。")]
        public void Should_Report_Size_Of_Group()
        {
            var sequenceGroup = new SequenceGroup();
            sequenceGroup.Add(new Sequence());
            sequenceGroup.Add(new Sequence());
            sequenceGroup.Add(new Sequence());

            Assert.Equal(3, sequenceGroup.Size());
        }

        [Fact(DisplayName = "应该能够成功移除存在的序号。")]
        public void Should_Remove_Sequence_From_Group()
        {
            var sequenceThree = new Sequence(3L);
            var sequenceSeven = new Sequence(7L);
            var sequenceGroup = new SequenceGroup();

            sequenceGroup.Add(sequenceSeven);
            sequenceGroup.Add(sequenceThree);

            Assert.Equal(2, sequenceGroup.Size());
            Assert.Equal(sequenceThree.GetValue(), sequenceGroup.GetMinimumSequence());

            Assert.True(sequenceGroup.Remove(sequenceThree));
            Assert.Equal(sequenceSeven.GetValue(), sequenceGroup.GetMinimumSequence());
            Assert.Equal(1, sequenceGroup.Size());
        }

        [Fact(DisplayName = "删除序号时，应该删除值相同的所有序号。")]
        public void Should_Remove_Sequence_From_Group_Where_It_Been_Added_Multiple_Times()
        {
            var sequenceThree = new Sequence(3L);
            var sequenceSeven = new Sequence(7L);
            var sequenceGroup = new SequenceGroup();

            sequenceGroup.Add(sequenceThree);
            sequenceGroup.Add(sequenceSeven);
            sequenceGroup.Add(sequenceThree);

            Assert.Equal(sequenceThree.GetValue(), sequenceGroup.GetMinimumSequence());

            Assert.True(sequenceGroup.Remove(sequenceThree));
            Assert.Equal(sequenceSeven.GetValue(), sequenceGroup.GetMinimumSequence());
            Assert.Equal(1, sequenceGroup.Size());
        }

        [Fact(DisplayName = "给序号组赋值，该组下所有序号的值都应该相同。")]
        public void Should_Set_Group_Sequence_To_Same_Value()
        {
            var sequenceThree = new Sequence(3L);
            var sequenceSeven = new Sequence(7L);
            var sequenceGroup = new SequenceGroup();

            sequenceGroup.Add(sequenceSeven);
            sequenceGroup.Add(sequenceThree);

            var expectedSequence = 11L;
            sequenceGroup.SetValue(expectedSequence);

            Assert.Equal(expectedSequence, sequenceThree.GetValue());
            Assert.Equal(expectedSequence, sequenceSeven.GetValue());
        }

        [Fact(DisplayName = "在线程开始发布到Disruptor后，添加序列到序列组。")]
        public void ShouldAddWhileRunning()
        {
            var ringBuffer = RingBuffer<TestEvent>.CreateSingleProducer(() => new TestEvent(), 32);
            var sequenceThree = new Sequence(3L);
            var sequenceSeven = new Sequence(7L);
            var sequenceGroup = new SequenceGroup();
            sequenceGroup.Add(sequenceSeven);

            for (int i = 0; i < 11; i++)
            {
                ringBuffer.Publish(ringBuffer.Next());
            }

            sequenceGroup.AddWhileRunning(ringBuffer, sequenceThree);
            Assert.Equal(10L, sequenceThree.GetValue());
        }
    }
}
