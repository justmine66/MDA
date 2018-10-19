using System;
using System.Threading;
using MDA.Disruptor.Utility;

namespace MDA.Disruptor.Impl
{
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

        public void AddWhileRunning(ICursored cursored, ISequence sequence)
        {
            SequenceGroupManager.AddSequences(ref _sequences, cursored, sequence);
        }

        public bool Remove(ISequence sequence)
        {
            return SequenceGroupManager.RemoveSequence(ref _sequences, sequence);
        }

        public long GetMinimumSequence()
        {
            return SequenceGroupManager.GetMinimumSequence(_sequences);
        }

        public void SetValue(long value)
        {
            foreach (var sequence in _sequences)
            {
                sequence.SetValue(value);
            }
        }

        public void SetVolatileValue(long value)
        {
            foreach (var sequence in _sequences)
            {
                sequence.SetVolatileValue(value);
            }
        }

        public int Size()
        {
            return _sequences.Length;
        }
    }
}
