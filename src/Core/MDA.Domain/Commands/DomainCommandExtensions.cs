using MDA.Domain.Events;
using MDA.Domain.Notifications;
using MDA.Domain.Saga;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        internal static IDomainCommand WithEventingContext(this IDomainCommand command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return command;
        }

        internal static IDomainCommand WithEventingContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainEvent @event)
        {
            command.ApplicationCommandId = @event.ApplicationCommandId;
            command.ApplicationCommandType = @event.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = @event.ApplicationCommandReplyScheme;

            return command;
        }

        public static IDomainCommand WithNotifyingContext(this IDomainCommand command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = notification.ApplicationCommandReplyScheme;

            return command;
        }

        public static IDomainCommand WithNotifyingContext<TAggregateRootId>(this IDomainCommand<TAggregateRootId> command, IDomainNotification notification)
        {
            command.ApplicationCommandId = notification.ApplicationCommandId;
            command.ApplicationCommandType = notification.ApplicationCommandType;
            command.ApplicationCommandReplyScheme = notification.ApplicationCommandReplyScheme;

            return command;
        }

        public static async Task OnResultAsync(this IEnumerable<DomainEventResult> results,
            Func<DomainEventResult, Task> successCallbackAsync,
            Func<DomainEventResult, Task> failCallbackAsync)
        {
            if (results == null)
            {
                return;
            }

            foreach (var result in results)
            {
                if (result.StorageSucceed() || result.HandleSucceed())
                {
                    await successCallbackAsync(result);
                }
                else
                {
                    await failCallbackAsync(result);
                }
            }
        }
    }
}
