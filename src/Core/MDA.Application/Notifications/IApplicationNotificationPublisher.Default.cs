using MDA.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Notifications
{
    public class DefaultApplicationNotificationPublisher : IApplicationNotificationPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public DefaultApplicationNotificationPublisher(
            IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Publish<TApplicationNotification>(TApplicationNotification notification)
            where TApplicationNotification : IApplicationNotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _messagePublisher.Publish(notification);
        }

        public async Task PublishAsync<TApplicationNotification>(
            TApplicationNotification notification,
            CancellationToken token = default)
            where TApplicationNotification : IApplicationNotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            await _messagePublisher.PublishAsync(notification, token);
        }
    }
}
