using MDA.Messaging.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Messaging.Extensions
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, bool IsReigisterInMemoryBus = false)
        {
            services.AddSingleton<MessagingOptions>();
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
