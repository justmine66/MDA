using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Events
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainEventCore(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventStateBackend, MemoryDomainEventStateBackend>();

            return services;
        }
    }
}
