using System;
using System.Threading;

namespace MDA.Disruptor.Utility
{
    /// <summary>
    /// Provides static methods for managing a <see cref="ISequenceGroup"/> object.
    /// </summary>
    internal static class SequenceGroupManager
    {
        internal static long GetMinimumSequence(ISequence[] sequences, long minimum = long.MaxValue)
        {
            for (int i = 0; i < sequences.Length; i++)
            {
                var value = sequences[i].GetValue();
                minimum = Math.Min(value, minimum);
            }

            return minimum;
        }

        internal static void AddSequences(ref ISequence[] sequences, ICursored cursor, params ISequence[] sequencesToAdd)
        {
            long cursorSequence;
            ISequence[] updatedSequences;
            ISequence[] currentSequences;

            do
            {
                currentSequences = Volatile.Read(ref sequences);
                var currentLength = currentSequences.Length;
                updatedSequences = new ISequence[currentLength + sequencesToAdd.Length];

                Array.Copy(currentSequences, updatedSequences, currentLength);
                cursorSequence = cursor.GetCursor();

                foreach (var sequence in sequencesToAdd)
                {
                    sequence.SetValue(cursorSequence);
                    updatedSequences[currentLength++] = sequence;
                }
            } while (Interlocked.CompareExchange(ref sequences, updatedSequences, currentSequences) != currentSequences);

            cursorSequence = cursor.GetCursor();
            foreach (var sequence in sequencesToAdd)
            {
                sequence.SetValue(cursorSequence);
            }
        }

        internal static bool RemoveSequence(ref ISequence[] sequences, ISequence sequenceToRemove)
        {
            int numToRemove;
            ISequence[] oldSequences;
            ISequence[] newSequences;

            do
            {
                oldSequences = Volatile.Read(ref sequences);
                numToRemove = CountMatching(oldSequences, sequenceToRemove);

                if (0 == numToRemove)
                {
                    break;
                }

                int oldSize = oldSequences.Length;
                newSequences = new ISequence[oldSize - numToRemove];

                for (int i = 0, pos = 0; i < oldSize; i++)
                {
                    var sequence = oldSequences[i];
                    if (sequence != sequenceToRemove)
                    {
                        newSequences[pos++] = sequence;
                    }
                }
            } while (Interlocked.CompareExchange(ref sequences, newSequences, oldSequences) != oldSequences);

            return numToRemove != 0;
        }

        private static int CountMatching(ISequence[] sequences, ISequence toMatch)
        {
            var count = 0;
            foreach (var sequence in sequences)
            {
                if (sequence == toMatch)
                {
                    count++;
                }
            }

            return count;
        }
    }
}
