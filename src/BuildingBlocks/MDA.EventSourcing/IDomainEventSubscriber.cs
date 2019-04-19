using System;

namespace MDA.EventSourcing
{
    public interface IDomainEventSubscriber<in T> where T : IDomainEvent
    {
        void HandleEvent(T domainEvent);
        Type SubscribedToEventType();
    }
}
