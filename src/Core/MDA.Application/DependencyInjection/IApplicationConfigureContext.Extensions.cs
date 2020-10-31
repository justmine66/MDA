using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;

namespace MDA.Application.DependencyInjection
{
    public static class ApplicationConfigureContextExtensions
    {
        public static IApplicationConfigureContext UseMessageBus(this IApplicationConfigureContext context, MessageBusClientNames name)
        {
            context.Services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            context.Services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            context.Services.AddTypedMessagePublisher<IApplicationNotificationPublisher, DefaultApplicationNotificationPublisher>(name);

            return context;
        }
    }
}
