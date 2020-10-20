using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Events
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainEventServices(this IServiceCollection services)
        {
            services.AddSingleton<IDomainEventStateBackend, MemoryDomainEventStateBackend>();
            services.AddSingleton<IDomainEventPublisher, DefaultDomainEventPublisher>();

            return services;
        }
    }
}
