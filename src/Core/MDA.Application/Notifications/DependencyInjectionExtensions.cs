using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.Application.Notifications
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationNotifications(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IApplicationNotificationPublisher, ApplicationNotificationPublisher>();

            return services;
        }
    }
}
