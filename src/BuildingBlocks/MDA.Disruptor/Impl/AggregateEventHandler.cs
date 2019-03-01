namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// An aggregate collection of <see cref="IEventHandler{TEvent}"/>s that get called in sequence for each event.
    /// </summary>
    /// <typeparam name="TEvent">implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public class AggregateEventHandler<TEvent> : IEventHandler<TEvent>, ILifecycleAware
    {
        private readonly IEventHandler<TEvent>[] _eventHandlers;

        /// <summary>
        /// Construct an aggregate collection of <see cref="IEventHandler{TEvent}"/>s to be called in sequence.
        /// </summary>
        /// <param name="eventHandlers">to be called in sequence.</param>
        public AggregateEventHandler(params IEventHandler<TEvent>[] eventHandlers)
        {
            _eventHandlers = eventHandlers;
        }

        public void OnEvent(TEvent @event, long sequence, bool endOfBatch)
        {
            foreach (var handler in _eventHandlers)
            {
                handler.OnEvent(@event, sequence, endOfBatch);
            }
        }

        public void OnShutdown()
        {
            foreach (var handler in _eventHandlers)
            {
                if (handler is ILifecycleAware aware)
                {
                    aware?.OnShutdown();
                }
            }
        }

        public void OnStart()
        {
            foreach (var handler in _eventHandlers)
            {
                if (handler is ILifecycleAware aware)
                {
                    aware?.OnStart();
                }
            }
        }
    }
}
