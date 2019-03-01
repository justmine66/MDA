using MDA.Disruptor.Exceptions;
using Microsoft.Extensions.Logging;
using System;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Convenience implementation of an exception handler that using standard logging to log the exception as <see cref="LogLevel.Critical"/> and re-throw it wrapped in a <see cref="RuntimeException"/>
    /// </summary>
    public class FatalExceptionHandler<TEvent> : IExceptionHandler<TEvent>
    {
        private readonly ILogger _logger;
        public FatalExceptionHandler(ILogger<FatalExceptionHandler<TEvent>> logger)
        {
            _logger = logger;
        }

        public void HandleEventException(Exception ex, long sequence, TEvent @event)
        {
            _logger.LogCritical("Exception processing: " + sequence + " " + @event, ex);
            throw new RuntimeException(ex);
        }

        public void HandleOnShutdownException(Exception ex)
        {
            _logger.LogCritical("Exception during onShutdown()", ex);
        }

        public void HandleOnStartException(Exception ex)
        {
            _logger.LogCritical("Exception during onStart()", ex);
        }
    }
}
