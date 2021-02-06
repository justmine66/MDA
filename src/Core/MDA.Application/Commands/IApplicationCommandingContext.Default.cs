using MDA.Application.Notifications;
using MDA.Domain.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandingContext : IApplicationCommandingContext
    {
        private readonly IDomainCommandPublisher _domainCommandPublisher;
        private readonly IApplicationNotificationPublisher _applicationNotificationPublisher;

        private IApplicationCommand _applicationCommand;

        public DefaultApplicationCommandingContext(
            IServiceProvider serviceProvider,
            IDomainCommandPublisher commandPublisher,
            IApplicationNotificationPublisher notificationPublisher)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException();
            _domainCommandPublisher = commandPublisher ?? throw new ArgumentNullException();
            _applicationNotificationPublisher = notificationPublisher ?? throw new ArgumentNullException();
        }

        public IServiceProvider ServiceProvider { get; }

        public void SetApplicationCommand(IApplicationCommand command)
        {
            _applicationCommand = command;
        }

        public void PublishDomainCommand(IDomainCommand command)
        {
            command.FillFrom(_applicationCommand);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command)
        {
            command.FillFrom(_applicationCommand);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishApplicationNotification<TApplicationNotification>(TApplicationNotification notification)
            where TApplicationNotification : IApplicationNotification
        {
            _applicationNotificationPublisher.Publish(notification);
        }

        public async Task PublishApplicationNotificationAsync<TApplicationNotification>(TApplicationNotification notification,
            CancellationToken token = default) where TApplicationNotification : IApplicationNotification
        {
            await _applicationNotificationPublisher.PublishAsync(notification, token);
        }
    }
}
