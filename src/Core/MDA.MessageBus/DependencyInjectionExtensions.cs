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
            services.AddSingleton<IMessageSubscriber, MessageSubscriber>();
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
            services.AddSingleton<IMessageBus, MessageBus>();
            //services.AddMessageHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>();
            assemblyList.AddRange(assemblies);

            var serviceType = typeof(IMessageHandler<>);
            var implementationTypes = assemblyList
                .SelectMany(assembly => assembly.GetTypes().Where(it => !it.IsInterface && !it.IsAbstract))
                .SelectMany(instance => instance.GetInterfaces())
                .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == serviceType);

            foreach (var implementationType in implementationTypes)
            {
                var concreteServiceType = serviceType.MakeGenericType(implementationType.GenericTypeArguments);

                services.AddTransient(concreteServiceType, implementationType);
            }

            return services;
        }
    }
}
