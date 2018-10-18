﻿using System;
using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// Interface for <see cref="BatchEventProcessor{TEvent,TDataProvider,TSequenceBarrier,TEventHandler,TBatchStartAware}"/>.
    /// </summary>
    /// <typeparam name="TEvent">the type of event used.</typeparam>
    public interface IBatchEventProcessor<TEvent> : IEventProcessor
    {
        /// <summary>
        /// Waits before the event processor enters the <see cref="IEventProcessor.IsRunning"/> state.
        /// </summary>
        /// <param name="timeout">maximum wait duration</param>
        void WaitUntilStarted(TimeSpan timeout);

        /// <summary>
        /// Set a new <see cref="IExceptionHandler{TEvent}"/> for handling exceptions propagated out of the <see cref="IEventHandler{TEvent}"/>
        /// </summary>
        /// <param name="exceptionHandler">exceptionHandler to replace the existing exceptionHandler.</param>
        void SetExceptionHandler(IExceptionHandler<TEvent> exceptionHandler);
    }
}
