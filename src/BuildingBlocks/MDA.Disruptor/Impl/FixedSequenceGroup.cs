using MDA.Disruptor.Utility;
using System;
using System.Linq;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Hides a group of Sequences behind a single Sequence.
    /// </summary>
    public class FixedSequenceGroup : ISequence
    {
        private readonly ISequence[] _sequences;

        public FixedSequenceGroup(ISequence[] sequences)
        {
            _sequences = sequences.ToArray();
        }

        public long AddAndGet(long increment)
        {
            throw new NotSupportedException();
        }

        public bool CompareAndSet(long expectedValue, long newValue)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get the minimum sequence value for the group.
        /// </summary>
        /// <returns></returns>
        public long GetValue()
        {
            return SequenceGroupManager.GetMinimumSequence(_sequences);
        }

        public long IncrementAndGet()
        {
            throw new NotSupportedException();
        }

        public void SetValue(long value)
        {
            throw new NotSupportedException();
        }

        public void SetVolatileValue(long value)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return string.Join(", ", _sequences.Select(it => it.ToString()));
        }
    }
}
