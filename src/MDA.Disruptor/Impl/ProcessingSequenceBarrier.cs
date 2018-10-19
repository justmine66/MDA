using MDA.Disruptor.Exceptions;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// <see cref="ISequenceBarrier"/> handed out for gating <see cref="IEventProcessor"/>s on a cursor sequence and optional dependent <see cref="IEventProcessor"/>(s), using the given WaitStrategy.
    /// </summary>
    public class ProcessingSequenceBarrier : ISequenceBarrier
    {
        private readonly IWaitStrategy _waitStrategy;
        private readonly ISequence _dependentSequence;
        private readonly ISequence _cursorSequence;
        private readonly ISequencer _sequencer;

        private volatile bool _alerted = false;

        public ProcessingSequenceBarrier(
         ISequencer sequencer,
         IWaitStrategy waitStrategy,
         ISequence cursorSequence,
         ISequence[] dependentSequences)
        {
            _sequencer = sequencer;
            _waitStrategy = waitStrategy;
            _cursorSequence = cursorSequence;
            if (0 == dependentSequences.Length)
            {
                _dependentSequence = cursorSequence;
            }
            else
            {
                _dependentSequence = new FixedSequenceGroup(dependentSequences);
            }
        }

        public bool IsAlerted => _alerted;

        public void Alert()
        {
            _alerted = true;
            _waitStrategy.SignalAllWhenBlocking();
        }

        public void CheckAlert()
        {
            if (_alerted)
            {
                throw AlertException.Instance;
            }
        }

        public void ClearAlert()
        {
            _alerted = false;
        }

        public long GetCursor()
        {
            return _dependentSequence.GetValue();
        }

        public long WaitFor(long sequence)
        {
            CheckAlert();

            var availableSequence = _waitStrategy.WaitFor(sequence, _cursorSequence, _dependentSequence, this);

            if (availableSequence < sequence)
            {
                return availableSequence;
            }

            return _sequencer.GetHighestPublishedSequence(sequence, availableSequence);
        }
    }
}
