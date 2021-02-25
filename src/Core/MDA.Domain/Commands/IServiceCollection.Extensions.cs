using MDA.Domain.Models;
using MDA.Domain.Shared.Commands;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MDA.Domain.Commands
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainCommandCore(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddTypedMessagePublisher<IDomainCommandPublisher, DefaultDomainCommandPublisher>(MessageBusClientNames.Disruptor);
            services.AddDomainCommandTransportMessageHandlers(assemblies);

            return services;
        }

        public static IServiceCollection AddDomainCommandTransportMessageHandlers(this IServiceCollection services, params Assembly[] assemblies)
        {
            var assemblyList = new List<Assembly>(assemblies);
            var aggregateRootTypes = assemblyList
                .SelectMany(assembly => assembly.GetTypes()
                    .Where(it => it.IsAggregateRoot()));

            var messageTypeDefinition = typeof(DomainCommandTransportMessage<>);
            var processorImplTypeDefinition = typeof(InBoundDomainCommandProcessor<>);
            var messageHandlerTypeDefinition = typeof(IMessageHandler<>);
            var messageHandlerProxyTypeDefinition = typeof(IMessageHandlerProxy<>);
            var messageHandlerProxyImplTypeDefinition = typeof(DefaultMessageHandlerProxy<>);

            foreach (var aggregateRootType in aggregateRootTypes)
            {
                var idType = aggregateRootType.GetAggregateRootIdType();

                var messageType = messageTypeDefinition.MakeGenericType(idType);
                var processorImplType = processorImplTypeDefinition.MakeGenericType(idType);
                var messageHandlerType = messageHandlerTypeDefinition.MakeGenericType(messageType);
                var messageHandlerProxyType= messageHandlerProxyTypeDefinition.MakeGenericType(messageType);
                var messageHandlerProxyImplType = messageHandlerProxyImplTypeDefinition.MakeGenericType(messageType);

                services.AddScoped(messageHandlerType, processorImplType);
                services.AddScoped(messageHandlerProxyType, messageHandlerProxyImplType);
            }

            return services;
        }
    }
}
