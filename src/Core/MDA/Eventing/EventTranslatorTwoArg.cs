using System;
using Disruptor;
using MDA.Commanding;
using MDA.EventSourcing;

namespace MDA.Eventing
{
    public abstract class EventTranslatorTwoArg<TDomainEvent, TCommand>
        : IEventTranslatorTwoArg<TDomainEvent, BusinessPrincipal, TCommand>
        where TCommand : Command
        where TDomainEvent : DomainEvent
    {
        public virtual void TranslateTo(TDomainEvent e, long sequence, BusinessPrincipal principal, TCommand command)
        {
            command.ProcessingTime = DateTime.Now;
            e.Principal = principal;
            e.OccurredOn = DateTime.Now;
            e.EventVersion = sequence;
            MapDomainEvent(e, command);
        }

        public abstract void MapDomainEvent(TDomainEvent domainEvent, TCommand command);
    }
}
