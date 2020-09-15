using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.Application.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationCommands(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<IApplicationCommandPublisher, ApplicationCommandPublisher>();
            services.AddSingleton(typeof(IMessageHandler<>), typeof(ApplicationCommandProcessor<>));

            return services;
        }
    }
}
