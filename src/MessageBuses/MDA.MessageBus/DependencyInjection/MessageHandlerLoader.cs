﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.MessageBus.DependencyInjection
{
    internal static class MessageHandlerLoader
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static void AddMessageHandlers(IServiceCollection services, params Assembly[] assemblies)
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
                if (type.IgnoreMessageHandler())
                {
                    continue;
                }

                foreach (var serviceType in type.GetInterfaces())
                {
                    if (!serviceType.IsGenericType)
                    {
                        continue;
                    }

                    var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                    var genericArguments = serviceType.GetGenericArguments();

                    if (genericTypeDefinition != messageHandlerType &&
                        genericTypeDefinition != asyncMessageHandlerType)
                    {
                        continue;
                    }

                    services.AddScoped(serviceType, type);

                    if (TryAddProxy(genericArguments,
                        genericTypeDefinition == asyncMessageHandlerType,
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

            if (!IsMdaMessageType(genericArguments))
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

        private static bool IsMdaMessageType(IEnumerable<Type> arguments)
            => arguments.All(argument => typeof(IMessage).IsAssignableFrom(argument));

        private static bool IgnoreMessageHandler(this Type serviceType)
            => serviceType.GetCustomAttributes(typeof(IgnoreMessageHandlerForDependencyInjection)).Any();
    }
}