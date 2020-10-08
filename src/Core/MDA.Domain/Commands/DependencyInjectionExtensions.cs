using MDA.Domain.Models;
using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Domain.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDomainCommands(this IServiceCollection services)
        {
            services.AddSingleton<IDomainCommandPublisher, DomainCommandPublisher>();
            services.AddScoped<IMessageHandler<DomainCommandTransportMessage>, AggregateRootMessageProcessor>();
            services.AddScoped<IAsyncMessageHandler<DomainCommandTransportMessage>, AggregateRootMessageProcessor>();

            return services;
        }
    }
}
