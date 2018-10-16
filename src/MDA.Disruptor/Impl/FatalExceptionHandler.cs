using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using MDA.Disruptor.Exceptions;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    ///  Convenience implementation of an exception handler that using standard logging to log the exception as <see cref="LogLevel.Critical"/> and re-throw it wrapped in a <see cref="RuntimeException"/>
    /// </summary>
    public class FatalExceptionHandler : IExceptionHandler<object>
    {
        private readonly ILogger _logger;
        public FatalExceptionHandler(ILogger<FatalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void HandleEventException(Exception ex, long sequence, object @event)
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
