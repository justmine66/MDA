using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Notifications
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainNotificationCore(this IServiceCollection services)
        {
            // services.AddSingleton<IDomainNotificationPublisher, DefaultDomainNotificationPublisher>();

            return services;
        }
    }
}
