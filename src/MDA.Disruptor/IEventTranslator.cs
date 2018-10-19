using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// Implementations translate (write) data representations into events claimed from the <see cref="RingBuffer{TEvent}"/>.
    /// </summary>
    /// <remarks>When publishing to the RingBuffer, provide an EventTranslator. The RingBuffer will select the next available event by sequence and provide it to the EventTranslator(which should update the event), before publishing the sequence update.</remarks>
    /// <typeparam name="TEvent">event implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public interface IEventTranslator<in TEvent>
    {
        /// <summary>
        /// Translate a data representation into fields set in given event.
        /// </summary>
        /// <param name="event">event into which the data should be translated.</param>
        /// <param name="sequence">sequence that is assigned to event.</param>
        void TranslateTo(TEvent @event, long sequence);
    }
}
