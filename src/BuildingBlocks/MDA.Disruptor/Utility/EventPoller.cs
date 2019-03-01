using MDA.Disruptor.Impl;

namespace MDA.Disruptor.Utility
{
    /// <summary>
    /// Experimental poll-based interface for the Disruptor.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public class EventPoller<TEvent>
    {
        private readonly IDataProvider<TEvent> _dataProvider;
        private readonly ISequencer _sequencer;
        private readonly ISequence _sequence;
        private readonly ISequence _gatingSequence;

        public EventPoller(IDataProvider<TEvent> dataProvider,
                           ISequencer sequencer,
                           ISequence sequence,
                           ISequence gatingSequence)
        {
            _dataProvider = dataProvider;
            _sequencer = sequencer;
            _sequence = sequence;
            _gatingSequence = gatingSequence;
        }

        public ISequence GetSequence()
        {
            return _sequence;
        }

        public static EventPoller<TEvent> NewInstance(
            IDataProvider<TEvent> dataProvider,
            ISequencer sequencer,
            ISequence sequence,
            ISequence cursorSequence,
            params ISequence[] gatingSequences)
        {
            ISequence gatingSequence;
            switch (gatingSequences.Length)
            {
                case 0:
                    gatingSequence = cursorSequence;
                    break;
                case 1:
                    gatingSequence = gatingSequences[0];
                    break;
                default:
                    gatingSequence = new FixedSequenceGroup(gatingSequences);
                    break;
            }

            return new EventPoller<TEvent>(dataProvider, sequencer, sequence, gatingSequence);
        }

        public PollState Poll(IHandler<TEvent> eventHandler)
        {
            var currentSequence = _sequence.GetValue();
            var nextSequence = currentSequence + 1;
            var availableSequence = _sequencer.GetHighestPublishedSequence(nextSequence, _gatingSequence.GetValue());

            if (nextSequence <= availableSequence)
            {
                var processedSequence = currentSequence;

                try
                {
                    bool processed;
                    do
                    {
                        var @event = _dataProvider.Get(nextSequence);
                        processed = eventHandler.OnEvent(@event, nextSequence, nextSequence == availableSequence);
                        processedSequence = nextSequence;
                        nextSequence++;
                    }
                    while (nextSequence <= availableSequence & processed);
                }
                finally
                {
                    _sequence.SetValue(processedSequence);
                }

                return PollState.Processing;
            }
            else if (_sequencer.GetCursor() >= nextSequence)
            {
                return PollState.Gating;
            }
            else
            {
                return PollState.Idle;
            }
        }

        public interface IHandler<in T>
        {
            bool OnEvent(T @event, long sequence, bool endOfBatch);
        }

        public enum PollState
        {
            Processing,
            Gating,
            Idle
        }
    }
}