using MDA.Eventing;
using MDA.EventSourcing;
using MDA.EventStoring;

namespace MDA.Persistence
{
    public class InboundEventJournaler<TDomainEvent> : IInBoundDomainEventHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent, new()
    {
        private readonly IEventStore _eventStore;

        public InboundEventJournaler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void OnEvent(TDomainEvent data, long sequence, bool endOfBatch)
        {
            _eventStore.AppendAsync(data);
        }
    }
}
