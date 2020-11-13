using MDA.Domain.Commands;
using System.Collections.Generic;

namespace MDA.Domain.Notifications
{
    public static class DomainNotificationExtensions
    {
        public static void FillDomainCommandInfo(this IEnumerable<IDomainNotification> domainNotifications, IDomainCommand command)
        {
            foreach (var notification in domainNotifications)
            {
                notification.DomainCommandId = command.Id;
                notification.DomainCommandType = command.GetType();
                notification.Topic = command.Topic;
                notification.PartitionKey = command.PartitionKey;
            }
        }
    }
}
