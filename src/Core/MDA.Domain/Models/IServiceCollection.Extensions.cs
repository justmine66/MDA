using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Models
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainModelCore(this IServiceCollection services, IConfiguration configuration = null)
        {
            services.AddSingleton<IAggregateRootFactory, DefaultAggregateRootFactory>();
            services.AddSingleton<IAggregateRootMemoryCache, LruAggregateRootMemoryCache>();
            services.AddSingleton<IAggregateRootStateBackend, DefaultAggregateRootStateBackend>();
            services.AddSingleton(typeof(IAggregateRootCheckpointStateBackend<>), typeof(MemoryAggregateRootCheckpointStateBackend<>));
            services.AddSingleton<IAggregateRootCheckpointManager, DefaultAggregateRootCheckpointManager>();

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

            services.Configure<CheckpointTriggerOptions>(_ => { });
            if (configuration != null)
            {
                services.Configure<CheckpointTriggerOptions>(configuration.GetSection(nameof(CheckpointTriggerOptions)));
            }

            return services;
        }
    }
}
