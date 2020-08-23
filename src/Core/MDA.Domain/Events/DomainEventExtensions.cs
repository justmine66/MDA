namespace MDA.Domain.Events
{
    public static class DomainEventExtensions
    {
        public static string GetTypeFullName(this IDomainEvent @event) 
            => @event.GetType().FullName;
    }
}
