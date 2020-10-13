using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.MessageBus
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageBusCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IMessageSubscriberManager, MessageSubscriberManager>();
            services.AddSingleton<IMessageHandlerProxyFinder, MessageHandlerProxyFinder>();
            services.AddSingleton<IAsyncMessageQueueService, NoOpAsyncMessageQueueService>();
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddMessageHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>();
            assemblyList.AddRange(assemblies);

            var types = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface && !it.IsAbstract));

            var messageHandlerType = typeof(IMessageHandler<>);
            var asyncMessageHandlerType = typeof(IAsyncMessageHandler<>);

            foreach (var type in types)
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (!interfaceType.IsGenericType)
                    {
                        continue;
                    }

                    var genericType = interfaceType.GetGenericTypeDefinition();

                    if (genericType == messageHandlerType)
                    {
                        var proxyType = typeof(MessageHandlerProxy<>).MakeGenericType(interfaceType.GetGenericArguments());

                        services.AddScoped(interfaceType, type);
                        services.AddScoped(typeof(IMessageHandlerProxy), proxyType);
                    }

                    if (genericType == asyncMessageHandlerType)
                    {
                        var proxyType = typeof(AsyncMessageHandlerProxy<>).MakeGenericType(interfaceType.GetGenericArguments());

                        services.AddScoped(interfaceType, type);
                        services.AddScoped(typeof(IAsyncMessageHandlerProxy), proxyType);
                    }
                }
            }

            return services;
        }
    }
}
