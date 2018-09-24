namespace MDA.Disruptor
{
    /// <summary>
    /// Experimental poll-based interface for the Disruptor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EventPoller<T>
    {
        private readonly IDataProvider<T> _dataProvider;
        private readonly ISequencer _sequencer;
        private readonly ISequence _sequence;
        private readonly ISequence _gatingSequence;

        public EventPoller(IDataProvider<T> dataProvider,
                           ISequencer sequencer,
                           ISequence sequence,
                           ISequence gatingSequence)
        {
            _dataProvider = dataProvider;
            _sequencer = sequencer;
            _sequence = sequence;
            _gatingSequence = gatingSequence;
        }

        public PollState Poll(Handler<T> eventHandler)
        {
            long currentSequence = _sequence.GetValue();
            long nextSequence = currentSequence + 1;
            long availableSequence = _sequencer.GetHighestPublishedSequence(nextSequence, _gatingSequence.GetValue());

            if (nextSequence <= availableSequence)
            {
                bool processNextEvent;
                long processedSequence = currentSequence;

                try
                {
                    do
                    {
                        T @event = _dataProvider.Get(nextSequence);
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

                return PollState.PROCESSING;
            }
            else if (_sequencer.GetCursor() >= nextSequence)
            {
                return PollState.GATING;
            }
            else
            {
                return PollState.IDLE;
            }
        }

        public interface Handler<T>
        {
            bool OnEvent(T @event, long sequence, bool endOfBatch);
        }
    }

    public enum PollState
    {
        PROCESSING,
        GATING,
        IDLE
    }
}