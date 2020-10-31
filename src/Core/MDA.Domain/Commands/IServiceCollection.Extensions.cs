using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainCommandCore(this IServiceCollection services)
        {
            services.AddTypedMessagePublisher<IDomainCommandPublisher, DefaultDomainCommandPublisher>(MessageBusClientNames.Disruptor);
            services.AddScoped<IMessageHandler<DomainCommandTransportMessage<string>>, InBoundDomainCommandProcessor<string>>();
            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage<string>>, DefaultMessageHandlerProxy<DomainCommandTransportMessage<string>>>();

            services.AddScoped<IMessageHandler<DomainCommandTransportMessage<long>>, InBoundDomainCommandProcessor<long>>();
            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage<long>>, DefaultMessageHandlerProxy<DomainCommandTransportMessage<long>>>();

            return services;
        }
    }
}
