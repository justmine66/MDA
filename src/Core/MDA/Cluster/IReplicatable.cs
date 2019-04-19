using MDA.Eventing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.Cluster
{
    public interface IReplicatable
    {
        Task<bool> ToSlave(InboundEvent @event);
        Task<bool> ToSlave(IEnumerable<InboundEvent> events);
    }
}
