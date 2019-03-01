using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    ///  Called by the <see cref="RingBuffer{TEvent}"/> to pre-populate all the events to fill the RingBuffer.
    /// </summary>
    /// <typeparam name="TEvent">event implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public interface IEventFactory<out TEvent>
    {
        /// <summary>
        /// Implementations should instantiate an event object, with all memory already allocated where possible.
        /// </summary>
        /// <returns></returns>
        TEvent NewInstance();
    }
}
