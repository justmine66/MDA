using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Events
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventStateBackend, MemoryDomainEventStateBackend>();

            return services;
        }
    }
}
