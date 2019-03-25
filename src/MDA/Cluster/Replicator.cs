using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Cluster
{
    public class Replicator : IReplicatable
    {
        public Task<bool> Replicate(InboundEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}
