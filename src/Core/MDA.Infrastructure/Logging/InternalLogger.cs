using Microsoft.Extensions.Logging;
using System;

namespace MDA.Infrastructure.Logging
{
    public class InternalLogger : ILogger
    {
        private readonly string _name;

        public InternalLogger(string name)
        {
            _name = name;
        }

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state, 
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    ConsoleLogger.LogDebug(_name,formatter(state, exception));
                    break;
                case LogLevel.Debug:
                    ConsoleLogger.LogDebug(_name, formatter(state, exception));
                    break;
                case LogLevel.Information:
                    ConsoleLogger.LogInfo(_name, formatter(state, exception));
                    break;
                case LogLevel.Warning:
                    ConsoleLogger.LogWarn(_name, formatter(state, exception));
                    break;
                case LogLevel.Error:
                    ConsoleLogger.LogError(_name, formatter(state, exception));
                    break;
                case LogLevel.Critical:
                    ConsoleLogger.LogFatal(_name, formatter(state, exception));
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => NoOpDisposable.Instance;

        sealed class NoOpDisposable : IDisposable
        {
            public static readonly NoOpDisposable Instance = new NoOpDisposable();

            NoOpDisposable()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
