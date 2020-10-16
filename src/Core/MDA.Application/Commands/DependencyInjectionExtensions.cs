using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Application.Commands
{
    public static class DependencyInjectionExtensions
    {
        private static readonly List<Type[]> AddedProxies = new List<Type[]>();
        private static readonly List<Type[]> AddedAsyncProxies = new List<Type[]>();

        public static IServiceCollection AddApplicationCommandServices(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.AddSingleton<IApplicationCommandPublisher, ApplicationCommandPublisher>();
            services.AddSingleton<IApplicationCommandExecutor, ApplicationCommandExecutor>();
            services.AddSingleton<IApplicationCommandContext, ApplicationCommandContext>();
            services.AddSingleton<IApplicationCommandService, ApplicationCommandService>();

            AddApplicationCommandHandlers(services, assemblies);

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

        public static IServiceCollection AddApplicationCommand<TApplicationCommand>(this IServiceCollection services)
            where TApplicationCommand : class, IApplicationCommand
        {
            services.AddScoped(typeof(IMessageHandler<TApplicationCommand>), typeof(ApplicationCommandProcessor<TApplicationCommand>));
            services.AddScoped(typeof(IAsyncMessageHandler<TApplicationCommand>), typeof(ApplicationCommandProcessor<TApplicationCommand>));

            services.AddScoped(typeof(IMessageHandlerProxy<TApplicationCommand>), typeof(MessageHandlerProxy<TApplicationCommand>));
            services.AddScoped(typeof(IAsyncMessageHandlerProxy<TApplicationCommand>), typeof(AsyncMessageHandlerProxy<TApplicationCommand>));

            return services;
        }

        private static IServiceCollection AddApplicationCommand(IServiceCollection services, Type[] genericArguments)
        {
            services.AddScoped(typeof(IMessageHandler<>).MakeGenericType(genericArguments), typeof(ApplicationCommandProcessor<>).MakeGenericType(genericArguments));
            services.AddScoped(typeof(IAsyncMessageHandler<>).MakeGenericType(genericArguments), typeof(ApplicationCommandProcessor<>).MakeGenericType(genericArguments));

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

                proxyImplType = typeof(AsyncMessageHandlerProxy<>).MakeGenericType(genericArguments);
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

                proxyImplType = typeof(MessageHandlerProxy<>).MakeGenericType(genericArguments);
                proxyServiceType = typeof(IMessageHandlerProxy<>).MakeGenericType(genericArguments);

                AddedProxies.Add(genericArguments);
            }

            return true;
        }
    }
}
