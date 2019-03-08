using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MDA.MessageBus
{
    public static class MessageBusServiceCollectionExtension
    {
        public static IServiceCollection AddMessageBusBasicServices(this IServiceCollection services, Action<MessageOptions> options)
        {
            return services
                .AddMessageBusBasicServices()
                .Configure(options);
        }

        public static IServiceCollection AddMessageBusBasicServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMessageBusBasicServices()
                .Configure<MessageOptions>(configuration.GetSection(nameof(MessageOptions)));
        }

        private static IServiceCollection AddMessageBusBasicServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton<IMessageSubscriberCollection, MessageSubscriberCollection>();
            services.AddSingleton<IMessageSubscriberManager, InMemoryMessageSubscriberManager>();
            return services;
        }
    }
}
