using Microsoft.Extensions.DependencyInjection;

namespace MDA.MessageBus
{
    public static class MessageBusServiceCollectionExtension
    {
        public static IServiceCollection AddMessageBusServices(this IServiceCollection services)
        {
            services.AddSingleton<MessageOptions>();
            services.AddSingleton<IMessageSubscriberCollection, MessageSubscriberCollection>();
            services.AddSingleton<IMessageSubscriberManager, InMemoryMessageSubscriberManager>();

            return services;
        }
    }
}
