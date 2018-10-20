using System;
using MDA.Disruptor.Exceptions;

namespace MDA.Disruptor
{
    /// <summary>
    /// Callback handler for uncaught exceptions in the event processing cycle of the <see cref="IBatchEventProcessor{TEvent}"/>
    /// </summary>
    public interface IExceptionHandler<in TEvent>
    {
        /// <summary>
        /// Strategy for handling uncaught exceptions when processing an event.
        /// </summary>
        /// <remarks>
        /// <p>If the strategy wishes to terminate further processing by the <see cref="IBatchEventProcessor{TEvent}"/> then it should throw a <see cref="RuntimeException"/>.</p>
        /// </remarks>
        /// <param name="ex">the exception that propagated from the <see cref="IEventHandler{TEvent}"/>.</param>
        /// <param name="sequence">of the event which cause the exception.</param>
        /// <param name="event">being processed when the exception occurred. This can be null.</param>
        void HandleEventException(Exception ex, long sequence, TEvent @event);

        /// <summary>
        /// Callback to notify of an exception during <see cref="ILifecycleAware.OnStart"/>.
        /// </summary>
        /// <param name="ex">throw during the starting process.</param>
        void HandleOnStartException(Exception ex);

        /// <summary>
        /// Callback to notify of an exception during <see cref="ILifecycleAware.OnShutdown"/>.
        /// </summary>
        /// <param name="ex">throw during the shutdown process.</param>
        void HandleOnShutdownException(Exception ex);
    }
}