using MDA.Domain.Commands;
using System.Collections.Generic;

namespace MDA.Domain.Events
{
    public static class DomainEventExtensions
    {
        public static bool IsValid(this IDomainEvent @event)
            => @event != null &&
               !string.IsNullOrWhiteSpace(@event.DomainCommandId) &&
               @event.DomainCommandType != null &&
               !string.IsNullOrWhiteSpace(@event.AggregateRootId) &&
               @event.AggregateRootType != null &&
               !string.IsNullOrWhiteSpace(@event.Id);

        public static string Print(this IDomainEvent @event)
        {
            return $"DomainCommandId: {@event?.DomainCommandId}, DomainCommandType: {@event?.DomainCommandType.FullName}, AggregateRootId: {@event?.AggregateRootId}, AggregateRootType: {@event?.AggregateRootType.FullName}, AggregateRootVersion: {@event?.AggregateRootVersion}, DomainEventId: {@event?.Id}";
        }

        public static void FillFrom(this IEnumerable<IDomainEvent> domainEvents, IDomainCommand command)
        {
            foreach (var domainEvent in domainEvents)
            {
                domainEvent.DomainCommandId = command.Id;
                domainEvent.DomainCommandType = command.GetType();
                domainEvent.Topic = command.Topic;
                domainEvent.PartitionKey = command.PartitionKey;
                domainEvent.ApplicationCommandId = command.ApplicationCommandId;
                domainEvent.ApplicationCommandType = command.ApplicationCommandType;
                domainEvent.ApplicationCommandReturnScheme = command.ApplicationCommandReturnScheme;
            }
        }
    }
}
