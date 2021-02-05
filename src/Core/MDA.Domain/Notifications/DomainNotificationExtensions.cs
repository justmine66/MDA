﻿using MDA.Domain.Commands;
using MDA.Domain.Saga;

namespace MDA.Domain.Notifications
{
    public static class DomainNotificationExtensions
    {
        public static bool NeedReplyApplicationCommand(this IDomainNotification notification, out IEndSubTransactionDomainNotification ensDomainNotification)
        {
            if (notification.ApplicationCommandReturnScheme == ApplicationCommandResultReturnSchemes.OnDomainCommandHandled && 
                notification is IEndSubTransactionDomainNotification endSubTransactionDomainNotification)
            {
                ensDomainNotification = endSubTransactionDomainNotification;

                return true;
            }

            ensDomainNotification = null;
            return false;
        }

        public static IDomainNotification FillFrom(this IDomainNotification notification, IDomainCommand command)
        {
            notification.AggregateRootType = command.AggregateRootType.FullName;
            notification.AggregateRootId = command.AggregateRootId;
            notification.DomainCommandId = command.Id;
            notification.DomainCommandType = command.GetType().FullName;
            notification.Topic = command.Topic;
            notification.PartitionKey = command.PartitionKey;
            notification.ApplicationCommandId = command.ApplicationCommandId;
            notification.ApplicationCommandType = command.ApplicationCommandType;
            notification.ApplicationCommandReturnScheme = command.ApplicationCommandReturnScheme;

            return notification;
        }

        public static IDomainNotification FillFrom(this IDomainNotification sink, IDomainNotification source)
        {
            sink.AggregateRootType = source.AggregateRootType;
            sink.AggregateRootId = source.AggregateRootId;
            sink.DomainCommandId = source.Id;
            sink.DomainCommandType = source.DomainCommandType;
            sink.Topic = source.Topic;
            sink.PartitionKey = source.PartitionKey;
            sink.ApplicationCommandId = source.ApplicationCommandId;
            sink.ApplicationCommandType = source.ApplicationCommandType;
            sink.ApplicationCommandReturnScheme = source.ApplicationCommandReturnScheme;

            return sink;
        }
    }
}
