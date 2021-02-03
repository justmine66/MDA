using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Application.Commands
{
    internal static class ServiceCollectionExtensions
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static IServiceCollection AddApplicationCommandCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IApplicationCommandExecutor, DefaultApplicationCommandExecutor>();
            services.AddSingleton<IApplicationCommandContext, DefaultApplicationCommandContext>();
            services.AddSingleton<IApplicationCommandService, DefaultApplicationCommandService>();
            services.AddSingleton<IApplicationCommandResultListener, ApplicationCommandResultProcessor>();

            services.AddApplicationCommandHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddApplicationCommandHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>(assemblies);
            var types = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface && !it.IsAbstract));

            var commandHandlerType = typeof(IApplicationCommandHandler<>);
            var asyncCommandHandlerType = typeof(IAsyncApplicationCommandHandler<>);

            foreach (var implementationType in types)
            {
                foreach (var serviceType in implementationType.GetInterfaces())
                {
                    if (!serviceType.IsGenericType)
                    {
                        continue;
                    }

                    var genericType = serviceType.GetGenericTypeDefinition();
                    if (genericType != commandHandlerType &&
                        genericType != asyncCommandHandlerType)
                    {
                        continue;
                    }

                    var isAsync = genericType == asyncCommandHandlerType;
                    var genericArguments = serviceType.GetGenericArguments();

                    // 1. 注册命令处理者
                    services.AddScoped(serviceType, implementationType);

                    // 2. 注册消息处理者
                    var messageHandlerServiceType = isAsync
                        ? typeof(IAsyncMessageHandler<>).MakeGenericType(genericArguments)
                        : typeof(IMessageHandler<>).MakeGenericType(genericArguments);
                    var messageHandlerImplementationType = typeof(ApplicationCommandProcessor<>).MakeGenericType(genericArguments);

                    services.AddScoped(messageHandlerServiceType, messageHandlerImplementationType);

                    if (TryAddProxy(genericArguments, isAsync,
                        out var proxyServiceType,
                        out var proxyImplType))
                    {
                        services.AddScoped(proxyServiceType, proxyImplType);
                    }
                }
            }

            return services;
        }

        private static bool IsApplicationCommandType(IEnumerable<Type> arguments)
            => arguments.All(argument => typeof(IApplicationCommand).IsAssignableFrom(argument));

        private static bool TryAddProxy(
            Type[] genericArguments,
            bool isAsync,
            out Type proxyServiceType,
            out Type proxyImplType)
        {
            proxyServiceType = default;
            proxyImplType = default;

            if (!IsApplicationCommandType(genericArguments))
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
    }
}
