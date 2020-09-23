using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Models
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainModels(this IServiceCollection services)
        {
            services.AddSingleton<IAggregateRootMemoryCache, LruAggregateRootMemoryCache>();
            services.AddSingleton<IAggregateRootStateBackend, DefaultAggregateRootStateBackend>();
            services.AddSingleton<IAggregateRootSavePointManager, MemoryAggregateRootSavePointManager>();

            return services;
        }
    }
}
