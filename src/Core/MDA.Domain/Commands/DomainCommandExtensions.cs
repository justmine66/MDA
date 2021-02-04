using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Domain.Saga;

namespace MDA.Domain.Commands
{
    public static class DomainCommandExtensions
    {
        public static bool NeedReplyApplicationCommand(this IDomainCommand command)
        {
            return !(command is ISubTransactionDomainCommand) && command.ApplicationCommandReturnScheme == ApplicationCommandResultReturnSchemes.OnDomainCommandHandled;
        }

        public static IDomainCommand WithContext(this IDomainCommand command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReturnScheme = @event.ApplicationCommandReturnScheme;

            return command;
        }

        public static IDomainCommand WithContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReturnScheme = @event.ApplicationCommandReturnScheme;

            return command;
        }

        public static IDomainCommand WithContext(this IDomainCommand command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReturnScheme = notification.ApplicationCommandReturnScheme;

            return command;
        }

        public static IDomainCommand WithContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReturnScheme = notification.ApplicationCommandReturnScheme;

            return command;
        }
    }
}
