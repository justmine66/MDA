using Disruptor;
using MDA.EventSourcing;

namespace MDA.Eventing
{
    public abstract class EventTranslatorTwoArg<TDomainEvent, TCommand> : IEventTranslatorTwoArg<TDomainEvent, string, TCommand>
        where TDomainEvent : DomainEvent
    {
        public virtual void TranslateTo(TDomainEvent e, long sequence, string principal, TCommand command)
        {
            e.Principal = principal;
            e.EventVersion = sequence;
            MapDomainEvent(e, command);
        }

        public abstract void MapDomainEvent(TDomainEvent domainEvent, TCommand command);
    }
}
