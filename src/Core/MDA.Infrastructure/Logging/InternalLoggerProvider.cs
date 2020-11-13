using Microsoft.Extensions.Logging;

namespace MDA.Infrastructure.Logging
{
    public sealed class InternalLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName) => new InternalLogger(categoryName);
    }
}
