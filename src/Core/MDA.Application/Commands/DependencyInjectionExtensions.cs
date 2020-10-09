using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MDA.Application.Commands
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationCommandCore(
            this IServiceCollection services, 
            params Assembly[] assemblies)
        {
            services.AddSingleton<IApplicationCommandPublisher, ApplicationCommandPublisher>();
            services.AddSingleton<IApplicationCommandExecutor, ApplicationCommandExecutor>();
            services.AddSingleton<IApplicationCommandContext, ApplicationCommandContext>();
            services.AddSingleton<IApplicationCommandService, ApplicationCommandService>();

            return services;
        }

        public static IServiceCollection AddApplicationCommand<TApplicationCommand>(
            this IServiceCollection services, 
            params Assembly[] assemblies)
            where TApplicationCommand : IApplicationCommand
        {
            services.AddScoped(typeof(IMessageHandler<TApplicationCommand>), typeof(ApplicationCommandProcessor<TApplicationCommand>));
            services.AddScoped(typeof(IAsyncMessageHandler<TApplicationCommand>), typeof(ApplicationCommandProcessor<TApplicationCommand>));

            return services;
        }
    }
}
