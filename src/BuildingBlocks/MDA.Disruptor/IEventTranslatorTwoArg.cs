using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// Implementations translate another data representations into events claimed from the <see cref="RingBuffer{TEvent}"/>.
    /// </summary>
    /// <typeparam name="TEvent">implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    /// <typeparam name="TArg0"></typeparam>
    /// <typeparam name="TArg1"></typeparam>
    public interface IEventTranslatorTwoArg<in TEvent, in TArg0, in TArg1>
    {
        /// <summary>
        /// Translate a data representation into fields set in given event.
        /// </summary>
        /// <param name="event">into which the data should be translated.</param>
        /// <param name="sequence">that is assigned to event.</param>
        /// <param name="arg0">The first user specified argument to the translator</param>
        /// <param name="arg1">The second user specified argument to the translator</param>
        void TranslateTo(TEvent @event, long sequence, TArg0 arg0, TArg1 arg1);
    }
}