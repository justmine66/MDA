using MDA.Domain.Commands;
using MDA.Domain.Exceptions;
using MDA.Domain.Notifications;
using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Events;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Events
{
    public class DefaultDomainEventingContext : IDomainEventingContext
    {
        public IServiceProvider ServiceProvider { get; }

        private readonly IDomainCommandPublisher _domainCommandPublisher;
        private readonly IDomainNotificationPublisher _domainNotificationPublisher;
        private readonly IDomainExceptionMessagePublisher _domainExceptionMessagePublisher;

        private IDomainEvent _domainEvent;

        public DefaultDomainEventingContext(
            IServiceProvider serviceProvider,
            IDomainCommandPublisher domainCommandPublisher,
            IDomainNotificationPublisher domainNotificationPublisher,
            IDomainExceptionMessagePublisher domainExceptionMessagePublisher)
        {
            ServiceProvider = serviceProvider;
            _domainCommandPublisher = domainCommandPublisher;
            _domainNotificationPublisher = domainNotificationPublisher;
            _domainExceptionMessagePublisher = domainExceptionMessagePublisher;
        }

        public void SetDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvent = domainEvent;
        }

        public void PublishDomainCommand(IDomainCommand command)
        {
            command.WithEventingContext(_domainEvent);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishDomainCommand<TAggregateRootId>(IDomainCommand<TAggregateRootId> command)
        {
            command.WithEventingContext(_domainEvent);

            _domainCommandPublisher.Publish(command);
        }

        public void PublishDomainNotification(IDomainNotification notification)
        {
            notification.WithEventingContext(_domainEvent);

            _domainNotificationPublisher.Publish(notification);
        }

        public void PublishDomainNotification<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification)
        {
            notification.WithEventingContext(_domainEvent);

            _domainNotificationPublisher.Publish(notification);
        }

        public async Task PublishDomainNotificationAsync(IDomainNotification notification, CancellationToken token = default)
        {
            notification.WithEventingContext(_domainEvent);

            await _domainNotificationPublisher.PublishAsync(notification, token);
        }

        public async Task PublishDomainNotificationAsync<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification,
            CancellationToken token = default)
        {
            notification.WithEventingContext(_domainEvent);

            await _domainNotificationPublisher.PublishAsync(notification, token);
        }

        public void PublishDomainExceptionMessage(IDomainExceptionMessage exception)
        {
            exception.WithEventingContext(_domainEvent);

            _domainExceptionMessagePublisher.Publish(exception);
        }

        public async Task PublishDomainExceptionMessageAsync(IDomainExceptionMessage exception, CancellationToken token = default)
        {
            exception.WithEventingContext(_domainEvent);

            await _domainExceptionMessagePublisher.PublishAsync(exception, token);
        }
    }
}