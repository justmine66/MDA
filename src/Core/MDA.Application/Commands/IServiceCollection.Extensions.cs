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

            services.AddApplicationCommandHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddApplicationCommandHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>();
            assemblyList.AddRange(assemblies);

            var types = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => !it.IsInterface && !it.IsAbstract));

            var commandHandlerType = typeof(IApplicationCommandHandler<>);
            var asyncCommandHandlerType = typeof(IAsyncApplicationCommandHandler<>);

            foreach (var type in types)
            {
                foreach (var serviceType in type.GetInterfaces())
                {
                    if (!serviceType.IsGenericType)
                    {
                        continue;
                    }

                    var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
                    var genericArguments = serviceType.GetGenericArguments();

                    if (genericTypeDefinition != commandHandlerType &&
                        genericTypeDefinition != asyncCommandHandlerType)
                    {
                        continue;
                    }

                    var isAsync = genericTypeDefinition == asyncCommandHandlerType;

                    services.AddScoped(serviceType, type);
                    services.AddScoped(
                        isAsync
                            ? typeof(IAsyncMessageHandler<>).MakeGenericType(genericArguments)
                            : typeof(IMessageHandler<>).MakeGenericType(genericArguments),
                        typeof(ApplicationCommandProcessor<>).MakeGenericType(genericArguments));
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
