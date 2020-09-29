using MDA.Application.Notifications;
using MDA.Domain.Commands;

namespace MDA.Application.Commands
{
    public class ApplicationCommandContext : IApplicationCommandContext
    {
        public ApplicationCommandContext(
            IDomainCommandPublisher provider,
            IApplicationNotificationPublisher applicationNotificationPublisher)
        {
            DomainCommandPublisher = provider;
            ApplicationNotificationPublisher = applicationNotificationPublisher;
        }


        public IDomainCommandPublisher DomainCommandPublisher { get; }

        public IApplicationNotificationPublisher ApplicationNotificationPublisher { get; }
    }
}
