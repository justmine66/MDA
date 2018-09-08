using MDA.Message.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Message
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, bool IsReigisterInMemoryBus = false)
        {
            services.AddSingleton<MessageOptions>();
            services.AddSingleton<IMessageSubscriberCollection, DefaultMessageSubscriberCollection>();
            services.AddSingleton<IMessageSubscriberManager, InMemoryMessageSubscriberManager>();

            if (IsReigisterInMemoryBus)
            {
                services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            }

            return services;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection services, MessageOptions options, bool IsReigisterInMemoryBus = false)
        {
            services.AddSingleton(options);
            services.AddSingleton<IMessageSubscriberCollection, DefaultMessageSubscriberCollection>();
            services.AddSingleton<IMessageSubscriberManager, InMemoryMessageSubscriberManager>();

            if (IsReigisterInMemoryBus)
            {
                services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            }

            return services;
        }
    }
}
