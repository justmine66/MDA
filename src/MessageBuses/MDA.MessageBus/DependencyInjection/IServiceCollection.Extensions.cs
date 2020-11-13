using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.MessageBus.DependencyInjection
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBusCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddHostedService<StartupHostedService>();
            services.AddSingleton<IMessageSubscriberManager, DefaultMessageSubscriberManager>();
            services.AddSingleton<IMessagePublisher, DefaultMessagePublisher>();
            services.AddSingleton<IMessageBus, DefaultMessageBus>();
            MessageHandlerLoader.AddMessageHandlers(services, assemblies);

            return services;
        }
    }
}
