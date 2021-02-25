using MDA.Domain.Saga;
using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Events;
using MDA.Domain.Shared.Notifications;

namespace MDA.Domain.Notifications
{
    public static class DomainNotificationExtensions
    {
        public static bool NeedReplyApplicationCommand(this IDomainNotification notification, out IEndSubTransactionDomainNotification needRepliedNotification)
        {
            if (notification.ApplicationCommandReplyScheme == Shared.ApplicationCommandReplySchemes.OnDomainCommandHandled && 
                notification is IEndSubTransactionDomainNotification endNotification)
            {
                needRepliedNotification = endNotification;

                return true;
            }

            needRepliedNotification = null;
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
            notification.ApplicationCommandReplyScheme = command.ApplicationCommandReplyScheme;

            return notification;
        }

        public static IDomainNotification FillFrom(this IDomainNotification notification, IDomainEvent @event)
        {
            notification.AggregateRootType = @event.AggregateRootType.FullName;
            notification.AggregateRootId = @event.AggregateRootId;
            notification.DomainCommandId = @event.Id;
            notification.DomainCommandType = @event.GetType().FullName;
            notification.Topic = @event.Topic;
            notification.PartitionKey = @event.PartitionKey;
            notification.ApplicationCommandId = @event.ApplicationCommandId;
            notification.ApplicationCommandType = @event.ApplicationCommandType;
            notification.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

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
            sink.ApplicationCommandReplyScheme = source.ApplicationCommandReplyScheme;

            return sink;
        }

        internal static IDomainNotification WithEventingContext(this IDomainNotification notification, IDomainEvent @event)
        {
            notification.ApplicationCommandId = @event.ApplicationCommandId;
            notification.ApplicationCommandType = @event.ApplicationCommandType;
            notification.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return notification;
        }

        internal static IDomainNotification WithEventingContext<TAggregateRootId>(this IDomainNotification<TAggregateRootId> notification, IDomainEvent @event)
        {
            notification.ApplicationCommandId = @event.ApplicationCommandId;
            notification.ApplicationCommandType = @event.ApplicationCommandType;
            notification.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return notification;
        }
    }
}
