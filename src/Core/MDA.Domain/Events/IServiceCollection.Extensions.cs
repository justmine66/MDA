using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Domain.Events
{
    internal static class ServiceCollectionExtensions
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static IServiceCollection AddDomainEventCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IDomainEventStateBackend, MemoryDomainEventStateBackend>();
            services.AddSingleton<IDomainEventHandlingContext, DefaultDomainEventHandlingContext>();
            services.AddDomainEventHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>(assemblies);
            var types = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface && !it.IsAbstract));

            var eventHandlerType = typeof(IDomainEventHandler<>);
            var asyncEventHandlerType = typeof(IAsyncDomainEventHandler<>);

            foreach (var implementationType in types)
            {
                foreach (var serviceType in implementationType.GetInterfaces())
                {
                    if (!serviceType.IsGenericType)
                    {
                        continue;
                    }

                    var genericType = serviceType.GetGenericTypeDefinition();
                    if (genericType != eventHandlerType &&
                        genericType != asyncEventHandlerType)
                    {
                        continue;
                    }

                    var isAsync = genericType == asyncEventHandlerType;
                    var genericArguments = serviceType.GetGenericArguments();

                    // 1. 注册命令处理者
                    services.AddScoped(serviceType, implementationType);

                    // 2. 注册消息处理者
                    var messageHandlerServiceType = isAsync
                        ? typeof(IAsyncMessageHandler<>).MakeGenericType(genericArguments)
                        : typeof(IMessageHandler<>).MakeGenericType(genericArguments);
                    var messageHandlerImplementationType = typeof(DomainEventProcessor<>).MakeGenericType(genericArguments);

                    services.AddScoped(messageHandlerServiceType, messageHandlerImplementationType);

                    if (TryAddProxy(genericArguments, isAsync,
                        out var proxyServiceType,
                        out var proxyImplementationType))
                    {
                        services.AddScoped(proxyServiceType, proxyImplementationType);
                    }
                }
            }

            return services;
        }

        private static bool IsDomainEventType(IEnumerable<Type> arguments)
            => arguments.All(argument => typeof(IDomainEvent).IsAssignableFrom(argument));

        private static bool TryAddProxy(
            Type[] genericArguments,
            bool isAsync,
            out Type proxyServiceType,
            out Type proxyImplementationType)
        {
            proxyServiceType = default;
            proxyImplementationType = default;

            if (!IsDomainEventType(genericArguments))
            {
                return false;
            }

            if (isAsync)
            {
                foreach (var proxyGenericArguments in AddedAsyncProxies)
                {
                    if (genericArguments.SequenceEqual(proxyGenericArguments))
                    {
                        return false;
                    }
                }

                proxyImplementationType = typeof(DefaultAsyncMessageHandlerProxy<>).MakeGenericType(genericArguments);
                proxyServiceType = typeof(IAsyncMessageHandlerProxy<>).MakeGenericType(genericArguments);

                AddedAsyncProxies.Add(genericArguments);
            }
            else
            {
                foreach (var proxyGenericArguments in AddedProxies)
                {
                    if (genericArguments.SequenceEqual(proxyGenericArguments))
                    {
                        return false;
                    }
                }

                proxyImplementationType = typeof(DefaultMessageHandlerProxy<>).MakeGenericType(genericArguments);
                proxyServiceType = typeof(IMessageHandlerProxy<>).MakeGenericType(genericArguments);

                AddedProxies.Add(genericArguments);
            }

            return true;
        }
    }
}
