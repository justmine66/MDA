using Microsoft.Extensions.DependencyInjection;

namespace MDA.Application.Notifications
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationNotificationServices(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationNotificationPublisher, ApplicationNotificationPublisher>();

            return services;
        }
    }
}
