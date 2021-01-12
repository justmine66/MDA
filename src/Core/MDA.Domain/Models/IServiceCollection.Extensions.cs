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

            services.Configure<AggregateRootOptions>(_ => { });
            services.Configure<AggregateRootCacheOptions>(_ => { });
            services.Configure<AggregateRootStateBackendOptions>(_ => { });
            services.Configure<CheckpointTriggerOptions>(_ => { });

            if (configuration == null) return services;

            var domainOptions = configuration.GetSection("MDA").GetSection("DomainOptions");
            var aggregateRootOptions = domainOptions.GetSection(nameof(AggregateRootOptions));

            services.Configure<AggregateRootOptions>(aggregateRootOptions);
            services.Configure<AggregateRootCacheOptions>(aggregateRootOptions.GetSection(nameof(AggregateRootOptions.CacheOptions)));
            services.Configure<AggregateRootStateBackendOptions>(aggregateRootOptions.GetSection(nameof(AggregateRootOptions.StateBackendOptions)));
            services.Configure<CheckpointTriggerOptions>(aggregateRootOptions.GetSection(nameof(AggregateRootOptions.CheckpointTriggerOptions)));

            return services;
        }
    }
}