using MDA.MessageBus;

namespace MDA.Domain.Events
{
    public interface IDomainEventHandler<in TDomainEvent> : IMessageHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    { }

    public interface IAsyncDomainEventHandler<in TDomainEvent> : IAsyncMessageHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    { }
}
