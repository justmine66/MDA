using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainCommandServices(this IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DomainCommandPublisher>();
            services.AddScoped<IMessageHandler<DomainCommandTransportMessage>, AggregateRootInBoundMessageProcessor>();
            //services.AddScoped<IAsyncMessageHandler<DomainCommandTransportMessage>, AggregateRootInBoundMessageProcessor>();

            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage>, MessageHandlerProxy<DomainCommandTransportMessage>>();
            //services.AddScoped<IAsyncMessageHandlerProxy<DomainCommandTransportMessage>, AsyncMessageHandlerProxy<DomainCommandTransportMessage>>();

            return services;
        }
    }
}
