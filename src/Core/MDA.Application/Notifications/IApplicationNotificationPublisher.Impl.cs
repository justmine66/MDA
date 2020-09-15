using MDA.MessageBus;
using System;

namespace MDA.Application.Notifications
{
    public class ApplicationNotificationPublisher : IApplicationNotificationPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public ApplicationNotificationPublisher(IMessagePublisher messagePublisher)
            => _messagePublisher = messagePublisher;

        public void Publish<TApplicationNotification>(TApplicationNotification notification)
            where TApplicationNotification : IApplicationNotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _messagePublisher.Publish(notification);
        }
    }
}
