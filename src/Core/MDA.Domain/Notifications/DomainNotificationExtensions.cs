using MDA.Domain.Commands;
using System.Collections.Generic;

namespace MDA.Domain.Notifications
{
    public static class DomainNotificationExtensions
    {
        public static IEnumerable<IDomainNotification> FillDomainCommandInfo(this IEnumerable<IDomainNotification> domainNotifications, IDomainCommand command)
        {
            foreach (var notification in domainNotifications)
            {
                yield return FillDomainCommandInfo(notification, command);
            }
        }

        public static IDomainNotification FillDomainCommandInfo(this IDomainNotification notification, IDomainCommand command)
        {
            notification.AggregateRootType = command.AggregateRootType;
            notification.AggregateRootId = command.AggregateRootId;
            notification.DomainCommandId = command.Id;
            notification.DomainCommandType = command.GetType();
            notification.Topic = command.Topic;
            notification.PartitionKey = command.PartitionKey;
            notification.ApplicationCommandId = command.ApplicationCommandId;
            notification.ApplicationCommandType = command.ApplicationCommandType;

            return notification;
        }
    }
}
