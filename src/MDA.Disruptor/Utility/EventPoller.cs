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

        public static EventPoller<TEvent> NewInstance(
            IDataProvider<TEvent> dataProvider,
            ISequencer sequencer,
            ISequence sequence,
            ISequence cursorSequence,
            params ISequence[] gatingSequences)
        {
            ISequence gatingSequence;
            if (gatingSequences.Length == 0)
            {
                gatingSequence = cursorSequence;
            }
            else if (gatingSequences.Length == 1)
            {
                gatingSequence = gatingSequences[0];
            }
            else
            {
                gatingSequence = new FixedSequenceGroup(gatingSequences);
            }

            return new EventPoller<TEvent>(dataProvider, sequencer, sequence, gatingSequence);
        }

        public PollState Poll(IHandler<TEvent> eventHandler)
        {
            long currentSequence = _sequence.GetValue();
            long nextSequence = currentSequence + 1;
            long availableSequence = _sequencer.GetHighestPublishedSequence(nextSequence, _gatingSequence.GetValue());

            if (nextSequence <= availableSequence)
            {
                long processedSequence = currentSequence;

                try
                {
                    bool processNextEvent;
                    do
                    {
                        TEvent @event = _dataProvider.Get(nextSequence);
                        processNextEvent = eventHandler.OnEvent(@event, nextSequence, nextSequence == availableSequence);
                        processedSequence = nextSequence;
                        nextSequence++;
                    }
                    while (nextSequence <= availableSequence & processNextEvent);
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
    }

    public enum PollState
    {
        Processing,
        Gating,
        Idle
    }
}