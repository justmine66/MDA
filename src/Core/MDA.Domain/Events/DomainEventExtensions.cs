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
    }
}
