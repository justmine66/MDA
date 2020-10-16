using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Models
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainModelServices(this IServiceCollection services, IConfiguration configuration = null)
        {
            services.AddSingleton<IAggregateRootFactory, AggregateRootFactory>();
            services.AddSingleton<IAggregateRootMemoryCache, LruAggregateRootMemoryCache>();
            services.AddSingleton<IAggregateRootStateBackend, DefaultAggregateRootStateBackend>();
            services.AddSingleton<IAggregateRootSavePointManager, MemoryAggregateRootSavePointManager>();

            services.Configure<AggregateRootCacheOptions>(_ => { });
            if (configuration != null)
            {
                services.Configure<AggregateRootCacheOptions>(configuration.GetSection(nameof(AggregateRootCacheOptions)));
            }

            services.Configure<AggregateRootStateBackendOptions>(_ => { });
            if (configuration != null)
            {
                services.Configure<AggregateRootStateBackendOptions>(configuration.GetSection(nameof(AggregateRootStateBackendOptions)));
            }

            return services;
        }
    }
}
