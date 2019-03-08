using MDA.Common.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Common.Concurrent
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConcurrentServices(this IServiceCollection container, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            container.AddServices<ICancellable, TimedCanceler>(lifetime);
            container.AddServices<IScheduledExecutor, ScheduledExecutorImpl>(lifetime);

            return container;
        }
    }
}
