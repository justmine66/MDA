using Microsoft.Extensions.Logging;
using System;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Convenience implementation of an exception handler that using standard logging to log the exception as <see cref="LogLevel.Information"/>. 
    /// </summary>
    public class IgnoreExceptionHandler : IExceptionHandler<object>
    {
        private readonly ILogger _logger;
        public IgnoreExceptionHandler(ILogger<IgnoreExceptionHandler> logger)
        {
            _logger = logger;
        }

        public void HandleEventException(Exception ex, long sequence, object @event)
        {
            _logger.LogInformation("Exception processing: " + sequence + " " + @event, ex);
        }

        public void HandleOnShutdownException(Exception ex)
        {
            _logger.LogInformation("Exception during onStart()", ex);
        }

        public void HandleOnStartException(Exception ex)
        {
            _logger.LogInformation("Exception during onShutdown()", ex);
        }
    }
}
