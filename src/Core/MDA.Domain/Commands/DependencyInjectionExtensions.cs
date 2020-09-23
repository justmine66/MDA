using System;
using MDA.Domain.Models;
using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DomainCommandPublisher>();
            services.AddTransient<IMessageHandler<DomainCommandTransportMessage>, AggregateRootMessageProcessor>();
            services.AddTransient<IAsyncMessageHandler<DomainCommandTransportMessage>, AggregateRootMessageProcessor>();

            return services;
        }

        public static IServiceProvider ConfigureDomainEvents(this IServiceProvider provider)
        {
            var subscriber = provider.GetService<IMessageSubscriber>();

            subscriber.Subscribe<DomainCommandTransportMessage, IMessageHandler<DomainCommandTransportMessage>>();
            //subscriber.Subscribe<DomainCommandTransportMessage, IAsyncMessageHandler<DomainCommandTransportMessage>>();

            return provider;
        }
    }
}
