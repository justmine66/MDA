using MDA.Eventing;

namespace MDA.Cluster
{
    public class InboundEventReplicator<T> : IInBoundEventHandler<T> where T : InboundEvent, new()
    {
        public void OnEvent(T data, long sequence, bool endOfBatch)
        {
           
        }
    }
}
