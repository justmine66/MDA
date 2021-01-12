using MDA.Application.Notifications;
using MDA.Domain.Commands;
using System;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandContext : IApplicationCommandContext
    {
        public DefaultApplicationCommandContext(
            IServiceProvider serviceProvider,
            IDomainCommandPublisher commandPublisher,
            IApplicationNotificationPublisher notificationPublisher)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException();
            DomainCommandPublisher = commandPublisher ?? throw new ArgumentNullException();
            ApplicationNotificationPublisher = notificationPublisher ?? throw new ArgumentNullException();
        }


        public IDomainCommandPublisher DomainCommandPublisher { get; }

        public IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
