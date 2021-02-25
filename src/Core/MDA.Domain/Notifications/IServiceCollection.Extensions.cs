using MDA.Domain.Shared.Notifications;
using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Domain.Notifications
{
    internal static class ServiceCollectionExtensions
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static IServiceCollection AddDomainNotificationCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IDomainNotifyingContext, DefaultDomainNotifyingContext>();
            services.AddDomainNotificationHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddDomainNotificationHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>(assemblies);
            var types = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface && !it.IsAbstract));

            var eventHandlerType = typeof(IDomainNotificationHandler<>);
            var asyncEventHandlerType = typeof(IAsyncDomainNotificationHandler<>);

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
                    var messageHandlerImplementationType = typeof(DomainNotificationProcessor<>).MakeGenericType(genericArguments);

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

        private static bool IsDomainNotificationType(IEnumerable<Type> arguments)
            => arguments.All(argument => typeof(IDomainNotification).IsAssignableFrom(argument));

        private static bool TryAddProxy(
            Type[] genericArguments,
            bool isAsync,
            out Type proxyServiceType,
            out Type proxyImplementationType)
        {
            proxyServiceType = default;
            proxyImplementationType = default;

            if (!IsDomainNotificationType(genericArguments))
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
