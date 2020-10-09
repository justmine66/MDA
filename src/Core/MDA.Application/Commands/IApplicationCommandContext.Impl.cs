using MDA.Application.Notifications;
using MDA.Domain.Commands;
using System;

namespace MDA.Application.Commands
{
    public class ApplicationCommandContext : IApplicationCommandContext
    {
        public ApplicationCommandContext(
            IServiceProvider serviceProvider,
            IDomainCommandPublisher commandPublisher,
            IApplicationNotificationPublisher notificationPublisher)
        {
            ServiceProvider = serviceProvider;
            DomainCommandPublisher = commandPublisher;
            ApplicationNotificationPublisher = notificationPublisher;
        }


        public IDomainCommandPublisher DomainCommandPublisher { get; }

        public IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
