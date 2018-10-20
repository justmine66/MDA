namespace MDA.Disruptor
{
    /// <summary>
    /// Used by the <see cref="IBatchEventProcessor{TEvent}"/> to set a callback allowing the <see cref="IEventHandler{TEvent}"/> to notify when it has finished consuming an event if this happens after the <see cref="IEventHandler{TEvent}.OnEvent(TEvent,long,bool)"/> call.
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface ISequenceReportingEventHandler<in TEvent> : IEventHandler<TEvent>
    {
        /// <summary>
        /// Call by the <see cref="IBatchEventProcessor{TEvent}"/> to setup the callback.
        /// </summary>
        /// <param name="sequenceCallback">callback on which to notify the <see cref="IBatchEventProcessor{TEvent}"/> that the sequence has progressed.</param>
        void SetSequenceCallback(ISequence sequenceCallback);
    }
}
