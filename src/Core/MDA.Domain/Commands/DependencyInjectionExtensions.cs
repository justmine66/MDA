using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainCommandServices(this IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DefaultDomainCommandPublisher>();

            services.AddScoped<IMessageHandler<DomainCommandTransportMessage<string>>, InBoundDomainCommandProcessor<string>>();
            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage<string>>, MessageHandlerProxy<DomainCommandTransportMessage<string>>>();

            services.AddScoped<IMessageHandler<DomainCommandTransportMessage<long>>, InBoundDomainCommandProcessor<long>>();
            services.AddScoped<IMessageHandlerProxy<DomainCommandTransportMessage<long>>, MessageHandlerProxy<DomainCommandTransportMessage<long>>>();

            return services;
        }
    }
}
