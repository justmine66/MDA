using MDA.Disruptor.Utility;
using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// A <see cref="Sequence"/> group that can dynamically have <see cref="Sequence"/>s added and removed while being thread safe.
    /// </summary>
    public class SequenceGroup : ISequenceGroup
    {
        private ISequence[] _sequences = new ISequence[0];

        public void Add(ISequence sequence)
        {
            ISequence[] oldSequences;
            ISequence[] newSequences;

            do
            {
                oldSequences = Volatile.Read(ref _sequences);
                var oldSize = oldSequences.Length;
                newSequences = new ISequence[oldSize + 1];
                Array.Copy(oldSequences, newSequences, oldSize);
                newSequences[oldSize] = sequence;
            } while (Interlocked.CompareExchange(ref _sequences, newSequences, oldSequences) != oldSequences);
        }

        public long AddAndGet(long increment)
        {
            throw new NotImplementedException();
        }

        public void AddWhileRunning(ICursored cursored, ISequence sequence)
        {
            throw new NotImplementedException();
        }

        public bool CompareAndSet(long expectedValue, long newValue)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool remove(ISequence sequence)
        {
            return SequenceGroupManager.RemoveSequence(ref _sequences, sequence);
        }

        /// <summary>
        ///  Set all <see cref="Sequence"/>s in the group to a given value.
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(long value)
        {
            for (int i = 0; i < _sequences.Length; i++)
            {
                _sequences[i].SetValue(value);
            }
        }

        public void SetVolatileValue(long value)
        {
            for (int i = 0; i < _sequences.Length; i++)
            {
                _sequences[i].SetVolatileValue(value);
            }
        }

        public int Size()
        {
            throw new NotImplementedException();
        }
    }
}
