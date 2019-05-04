using MDA.Eventing;
using MDA.EventSourcing;

namespace MDA.Cluster
{
    public class InboundEventReplicator<TDomainEvent> : IInBoundDomainEventHandler<TDomainEvent> 
        where TDomainEvent : IDomainEvent, new()
    {
        public void OnEvent(TDomainEvent data, long sequence, bool endOfBatch)
        {
           
        }
    }
}
