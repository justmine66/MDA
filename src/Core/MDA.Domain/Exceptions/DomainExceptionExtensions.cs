using MDA.Domain.Shared.Commands;
using MDA.Domain.Shared.Events;
using MDA.Domain.Shared.Exceptions;
using MDA.Domain.Shared.Notifications;

namespace MDA.Domain.Exceptions
{
    public static class DomainExceptionExtensions
    {
        public static IDomainExceptionMessage FillFrom(this IDomainExceptionMessage exception, IDomainCommand command)
        {
            exception.AggregateRootType = command.AggregateRootType.FullName;
            exception.AggregateRootId = command.AggregateRootId;
            exception.DomainCommandId = command.Id;
            exception.DomainCommandType = command.GetType().FullName;
            exception.Topic = command.Topic;
            exception.PartitionKey = command.PartitionKey;
            exception.ApplicationCommandId = command.ApplicationCommandId;
            exception.ApplicationCommandType = command.ApplicationCommandType;
            exception.ApplicationCommandReplyScheme = command.ApplicationCommandReplyScheme;

            return exception;
        }

        internal static IDomainExceptionMessage WithEventingContext(this IDomainExceptionMessage exception, IDomainEvent @event)
        {
            exception.ApplicationCommandId = @event.ApplicationCommandId;
            exception.ApplicationCommandType = @event.ApplicationCommandType;
            exception.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return exception;
        }

        internal static IDomainExceptionMessage WithNotifyingContext(this IDomainExceptionMessage exception, IDomainNotification notification)
        {
            exception.ApplicationCommandId = notification.ApplicationCommandId;
            exception.ApplicationCommandType = notification.ApplicationCommandType;
            exception.ApplicationCommandReplyScheme = notification.ApplicationCommandReplyScheme;

            return exception;
        }
    }
}
