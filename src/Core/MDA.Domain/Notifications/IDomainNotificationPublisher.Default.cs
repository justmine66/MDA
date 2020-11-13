using MDA.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace MDA.Domain.Notifications
{
    public class DefaultDomainNotificationPublisher : IDomainNotificationPublisher
    {
        private readonly DomainNotificationOptions _options;
        private readonly IMessagePublisher _messagePublisher;

        public DefaultDomainNotificationPublisher(
            IMessagePublisher messagePublisher, 
            IOptions<DomainNotificationOptions> options)
        {
            _messagePublisher = messagePublisher;
            _options = options.Value;
        }

        public void Publish(IDomainNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            notification.Topic = _options.Topic;

            _messagePublisher.Publish(notification);
        }

        public void Publish<TAggregateRootId>(IDomainNotification<TAggregateRootId> notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            notification.Topic = _options.Topic;

            _messagePublisher.Publish(notification);
        }

        public async Task PublishAsync(
            IDomainNotification notification, 
            CancellationToken token)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            notification.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(notification, token);
        }

        public async Task PublishAsync<TAggregateRootId>(
            IDomainNotification<TAggregateRootId> notification,
            CancellationToken token)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            notification.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(notification, token);
        }
    }
}
