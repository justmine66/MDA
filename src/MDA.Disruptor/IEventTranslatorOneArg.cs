using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// Implementations translate another data representations into events claimed from the <see cref="RingBuffer{TEvent}"/>
    /// </summary>
    /// <typeparam name="TEvent">implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    /// <typeparam name="TArg"></typeparam>
    public interface IEventTranslatorOneArg<in TEvent, in TArg>
    {
        /// <summary>
        /// Translate a data representation into fields set in given event.
        /// </summary>
        /// <param name="event">into which the data should be translated.</param>
        /// <param name="sequence">that is assigned to event.</param>
        /// <param name="arg">The first user specified argument to the translator.</param>
        void TranslateTo(TEvent @event, long sequence, TArg arg);
    }
}