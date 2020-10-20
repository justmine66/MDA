using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainCommandServices(this IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DomainCommandPublisher>();
            services.AddScoped<IMessageHandler<DomainCommandTransportMessage>, InBoundDomainCommandProcessor>();
            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage>, MessageHandlerProxy<DomainCommandTransportMessage>>();

            return services;
        }
    }
}
