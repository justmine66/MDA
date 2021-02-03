using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.MessageBus.DependencyInjection
{
    public static class MessageHandlerLoader
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static void LoadMessageHandlers(IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>(assemblies);
            var implementationTypes = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface &&
                                 !it.IsAbstract &&
                                 !it.IgnoreMessageHandler()));

            foreach (var implementationType in implementationTypes)
            {
                foreach (var serviceType in implementationType.GetInterfaces())
                {
                    if (!serviceType.IsMessageHandlerType()) continue;

                    var genericType = serviceType.GetGenericTypeDefinition();
                    var genericArguments = serviceType.GetGenericArguments();

                    services.AddScoped(serviceType, implementationType);

                    if (TryAddProxy(genericArguments,
                        genericType == typeof(IAsyncMessageHandler<>),
                        out var proxyServiceType,
                        out var proxyImplType))
                    {
                        services.AddScoped(proxyServiceType, proxyImplType);
                    }
                }
            }
        }

        private static bool TryAddProxy(
            Type[] genericArguments,
            bool isAsync,
            out Type proxyServiceType,
            out Type proxyImplType)
        {
            proxyServiceType = default;
            proxyImplType = default;

            if (!IsMessageType(genericArguments))
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

                proxyImplType = typeof(DefaultAsyncMessageHandlerProxy<>).MakeGenericType(genericArguments);
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

                proxyImplType = typeof(DefaultMessageHandlerProxy<>).MakeGenericType(genericArguments);
                proxyServiceType = typeof(IMessageHandlerProxy<>).MakeGenericType(genericArguments);

                AddedProxies.Add(genericArguments);
            }

            return true;
        }

        public static bool IsMessageType(IEnumerable<Type> arguments)
            => arguments.All(argument => typeof(IMessage).IsAssignableFrom(argument));

        public static bool IsMessageHandlerType(this Type type)
        {
            if (!type.IsGenericType) return false;

            var genericType = type.GetGenericTypeDefinition();

            return genericType == typeof(IMessageHandler<>) || genericType == typeof(IAsyncMessageHandler<>);
        }

        public static bool IgnoreMessageHandler(this Type type)
            => type.GetCustomAttributes(typeof(IgnoreMessageHandlerForDependencyInjection)).Any();
    }
}
