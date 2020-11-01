using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Application.DependencyInjection
{
    public static class ApplicationConfigureContextExtensions
    {
        public static IApplicationConfigureContext UseMessageBus(
            this IApplicationConfigureContext context, 
            MessageBusClientNames name, 
            IConfiguration configuration = null)
        {
            context.Services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            context.Services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            context.Services.AddTypedMessagePublisher<IApplicationNotificationPublisher, DefaultApplicationNotificationPublisher>(name);

            context.Services.Configure<ApplicationCommandOptions>(_ => { });
            if (configuration != null)
            {
                context.Services.Configure<ApplicationCommandOptions>(configuration.GetSection(nameof(ApplicationCommandOptions)));
            }

            return context;
        }
    }
}
