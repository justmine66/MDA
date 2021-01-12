using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.MessageBus.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageBusCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddHostedService<StartupHostedService>();
            services.AddSingleton<IMessageSubscriberManager, DefaultMessageSubscriberManager>();
            services.AddSingleton<IMessagePublisher, DefaultMessagePublisher>();
            services.AddSingleton<IMessageBus, DefaultMessageBus>();

            MessageHandlerLoader.LoadMessageHandlers(services, assemblies);

            return services;
        }

        public static IServiceCollection AddMessageHandler<TMessage, TMessageHandler>(this IServiceCollection services)
            where TMessage : class, IMessage
            where TMessageHandler : class, IMessageHandler<TMessage>
        {
            services.AddScoped<IMessageHandler<TMessage>, TMessageHandler>();
            services.AddScoped<IMessageHandlerProxy<TMessage>, DefaultMessageHandlerProxy<TMessage>>();

            return services;
        }

        public static IServiceCollection AddAsyncMessageHandler<TMessage, TAsyncMessageHandler>(this IServiceCollection services)
            where TMessage : class, IMessage
            where TAsyncMessageHandler : class, IAsyncMessageHandler<TMessage>
        {
            services.AddScoped<IAsyncMessageHandler<TMessage>, TAsyncMessageHandler>();
            services.AddScoped<IAsyncMessageHandlerProxy<TMessage>, DefaultAsyncMessageHandlerProxy<TMessage>>();

            return services;
        }
    }
}
