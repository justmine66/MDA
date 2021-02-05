using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Domain.Saga;

namespace MDA.Domain.Commands
{
    public static class DomainCommandExtensions
    {
        public static bool NeedReplyApplicationCommand(this IDomainCommand command)
        {
            var needReply = command.ApplicationCommandReplyScheme == ApplicationCommandReplySchemes.OnDomainCommandHandled;
            if (!needReply)
            {
                return false;
            }

            var isEnd = command is IEndSubTransactionDomainCommand;
            if (isEnd)
            {
                return true;
            }

            var isBegin = command is IBeginSubTransactionDomainCommand;
            if (isBegin)
            {
                return false;
            }

            return !(command is ISubTransactionDomainCommand);
        }

        public static IDomainCommand WithContext(this IDomainCommand command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return command;
        }

        public static IDomainCommand WithContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return command;
        }

        public static IDomainCommand WithContext(this IDomainCommand command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = notification.ApplicationCommandReplyScheme;

            return command;
        }

        public static IDomainCommand WithContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = notification.ApplicationCommandReplyScheme;

            return command;
        }
    }
}
