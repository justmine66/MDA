using MDA.Application.Commands;
using MDA.Application.Notifications;
using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Saga;
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
            var services = context.Services;

            services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            services.AddTypedMessagePublisher<IApplicationCommandPublisher, DefaultApplicationCommandPublisher>(name);
            services.AddTypedMessagePublisher<IApplicationNotificationPublisher, DefaultApplicationNotificationPublisher>(name);

            services.AddMessageHandler<DomainExceptionMessage, ApplicationCommandResultProcessor>();
            services.AddAsyncMessageHandler<DomainExceptionMessage, ApplicationCommandResultProcessor>();
            services.AddMessageHandler<SagaTransactionDomainNotification, ApplicationCommandResultProcessor>();
            services.AddAsyncMessageHandler<SagaTransactionDomainNotification, ApplicationCommandResultProcessor>();
            services.AddMessageHandler<DomainCommandHandledNotification, ApplicationCommandResultProcessor>();
            services.AddAsyncMessageHandler<DomainCommandHandledNotification, ApplicationCommandResultProcessor>();

            services.Configure<ApplicationCommandOptions>(_ => { });

            if (configuration == null) return context;

            var applicationOptions = configuration.GetSection("MDA").GetSection("ApplicationOptions");

            services.Configure<ApplicationCommandOptions>(applicationOptions.GetSection("CommandOptions"));

            return context;
        }
    }
}
