using MDA.Disruptor.Exceptions;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary p="This is useful in tests or for pre-filling a <see cref="RingBuffer"/> from a publisher.">
    /// No operation version of a <see cref="IEventProcessor"/> that simply tracks a <see cref="ISequence"/>.
    /// </summary>
    public class NoOpEventProcessor<T> : IEventProcessor
    {
        private readonly SequencerFollowingSequence _sequence;

        private volatile int _running = 0;

        private NoOpEventProcessor(RingBuffer<T> sequencer)
        {
            _sequence = new SequencerFollowingSequence(sequencer);
        }

        public ISequence GetSequence()
        {
            return _sequence;
        }

        public void Halt()
        {
            Interlocked.Exchange(ref _running, 0);
        }

        public bool IsRunning()
        {
            return _running == 1;
        }

        public void Run()
        {
            if (Interlocked.CompareExchange(ref _running, 0, 1) == 1)
            {
                throw new IllegalStateException("Thread is already running.");
            }
        }

        /// <summary>
        /// Sequence that follows (by wrapping) another sequence.
        /// </summary>
        private sealed class SequencerFollowingSequence : Sequence
        {
            private readonly RingBuffer<T> _sequencer;

            public SequencerFollowingSequence(RingBuffer<T> sequencer)
            {
                _sequencer = sequencer;
            }

            public override long GetValue()
            {
                return _sequencer.GetCursor();
            }
        }
    }
}
