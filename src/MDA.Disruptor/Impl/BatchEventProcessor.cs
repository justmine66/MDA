using MDA.Disruptor.Exceptions;
using System;
using System.Threading;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Convenience class for handling the batching semantics of consuming entries from a <see cref="RingBuffer{TEvent}"/> and delegating the available events to an <see cref="IEventHandler{TEvent}"/>.
    /// </summary>
    /// <remarks>
    /// If the <see cref="IEventHandler{TEvent}"/> also implements <see cref="ILifecycleAware"/> it will be notified just after the thread is started and just before the thread is shutdown.
    /// </remarks>
    /// <typeparam name="TEvent">implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public class BatchEventProcessor<TEvent>
        : IBatchEventProcessor<TEvent> where TEvent : class
    {

        private const int Idle = 0;
        private const int Halted = Idle + 1;
        private const int Running = Halted + 1;

        private volatile int _running;
        private IExceptionHandler<TEvent> _exceptionHandler;

        private readonly IDataProvider<TEvent> _dataProvider;
        private readonly ISequenceBarrier _sequenceBarrier;
        private readonly IEventHandler<TEvent> _eventHandler;
        private readonly ISequence _sequence;
        private readonly ITimeoutHandler _timeoutHandler;
        private readonly IBatchStartAware _batchStartAware;

        /// <summary>
        ///  Construct a <see cref="IBatchEventProcessor{TEvent}"/> that will automatically track the progress by updating its sequence when the <see cref="IEventHandler{TEvent}.OnEvent(TEvent,long,bool)"/> method returns.
        /// </summary>
        /// <param name="exceptionHandler"></param>
        /// <param name="dataProvider">to which events are published.</param>
        /// <param name="sequenceBarrier">on which it is waiting.</param>
        /// <param name="eventHandler">is the delegate to which events are dispatched.</param>
        public BatchEventProcessor(
            IDataProvider<TEvent> dataProvider,
            ISequenceBarrier sequenceBarrier,
            IEventHandler<TEvent> eventHandler,
            IExceptionHandler<TEvent> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            _dataProvider = dataProvider;
            _sequenceBarrier = sequenceBarrier;
            _eventHandler = eventHandler;
            _sequence = new Sequence();

            if (eventHandler is ISequenceReportingEventHandler<TEvent> reporting)
            {
                reporting?.SetSequenceCallback(_sequence);
            }

            if (eventHandler is IBatchStartAware aware && aware != null)
            {
                _batchStartAware = aware;
            }

            if (eventHandler is ITimeoutHandler timeout && timeout != null)
            {
                _timeoutHandler = timeout;
            }
        }

        public ISequence GetSequence()
        {
            return _sequence;
        }

        public void Halt()
        {
            Interlocked.Exchange(ref _running, Halted);
            _sequenceBarrier.Alert();
        }

        public void Run()
        {
            if (Interlocked.CompareExchange(ref _running, Running, Idle) == Idle)
            {
                _sequenceBarrier.ClearAlert();
                NotifyStart();

                try
                {
                    if (_running == Running)
                    {
                        ProcessEvents();
                    }
                }
                finally
                {
                    NotifyShutdown();
                    Interlocked.Exchange(ref _running, Idle);
                }
            }
            else
            {
                if (_running == Running)
                {
                    throw new IllegalStateException("Thread is already running.");
                }
                else
                {
                    EarlyExit();
                }
            }
        }

        public bool IsRunning()
        {
            return _running != Idle;
        }

        /// <summary>
        /// Set a new <see cref="IExceptionHandler{TEvent}"/> for handling exceptions propagated out of the <see cref="IBatchEventProcessor{TEvent}"/>.
        /// </summary>
        /// <param name="exceptionHandler">to replace the existing exceptionHandler.</param>
        public void SetExceptionHandler(IExceptionHandler<TEvent> exceptionHandler)
        {
            _exceptionHandler = exceptionHandler ?? throw new ArgumentNullException(nameof(exceptionHandler));
        }

        #region [ private methods ]

        private void ProcessEvents()
        {
            TEvent @event = null;
            var nextSequence = _sequence.GetValue() + 1L;

            while (true)
            {
                try
                {
                    var availableSequence = _sequenceBarrier.WaitFor(nextSequence);

                    _batchStartAware?.OnBatchStart(availableSequence - nextSequence + 1);

                    while (nextSequence <= availableSequence)
                    {
                        @event = _dataProvider.Get(nextSequence);
                        _eventHandler.OnEvent(@event, nextSequence, nextSequence == availableSequence);
                        nextSequence++;
                    }

                    _sequence.SetValue(availableSequence);
                }
                catch (Exceptions.TimeoutException e)
                {
                    NotifyTimeout(_sequence.GetValue());
                }
                catch (AlertException e)
                {
                    if (_running != Running)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    _exceptionHandler.HandleEventException(e, nextSequence, @event);
                    _sequence.SetValue(nextSequence);
                    nextSequence++;
                }
            }
        }

        private void EarlyExit()
        {
            NotifyStart();
            NotifyShutdown();
        }

        /// <summary>
        /// Notifies the EventHandler when this processor is starting up.
        /// </summary>
        private void NotifyStart()
        {
            if (!(_eventHandler is ILifecycleAware aware)) return;

            try
            {
                aware?.OnStart();
            }
            catch (Exception e)
            {
                _exceptionHandler.HandleOnStartException(e);
            }
        }

        /// <summary>
        /// Notifies the EventHandler immediately prior to this processor shutting down.
        /// </summary>
        private void NotifyShutdown()
        {
            if (!(_eventHandler is ILifecycleAware aware)) return;

            try
            {
                aware?.OnShutdown();
            }
            catch (Exception e)
            {
                _exceptionHandler.HandleOnShutdownException(e);
            }
        }

        private void NotifyTimeout(long availableSequence)
        {
            try
            {
                _timeoutHandler?.OnTimeout(availableSequence);
            }
            catch (Exception e)
            {
                _exceptionHandler?.HandleEventException(e, availableSequence, null);
            }
        }

        #endregion
    }
}
