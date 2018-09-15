using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansSiloHost
{
    public class LoggingCallFilter : IIncomingGrainCallFilter
    {
        private readonly ILogger _log;

        public LoggingCallFilter(ILogger<LoggingCallFilter> logger)
        {
            _log = logger;
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                await context.Invoke();

                var msg = string.Format(
                "{0}.{1}({2}) returned value {3}",
                context.Grain.GetType(),
                context.InterfaceMethod.Name,
                string.Join(", ", context.Arguments),
                context.Result);

                _log.LogInformation(msg);
            }
            catch (Exception e)
            {
                var msg = string.Format(
                "{0}.{1}({2}) threw an exception: {3}",
                context.Grain.GetType(),
                context.InterfaceMethod.Name,
                string.Join(", ", context.Arguments),
                e);

                _log.LogInformation(msg);

                throw;
            }
        }
    }
}
