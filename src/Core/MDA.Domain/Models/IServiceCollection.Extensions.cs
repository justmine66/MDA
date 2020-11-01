using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Models
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainModelCore(this IServiceCollection services, IConfiguration configuration = null)
        {
            services.AddSingleton<IAggregateRootFactory, AggregateRootFactory>();
            services.AddSingleton<IAggregateRootMemoryCache, LruAggregateRootMemoryCache>();
            services.AddSingleton<IAggregateRootStateBackend, DefaultAggregateRootStateBackend>();
            services.AddSingleton<IAggregateRootCheckpointManager, MemoryAggregateRootCheckpointManager>();

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
