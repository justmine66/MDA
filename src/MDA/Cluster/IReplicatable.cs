using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Cluster
{
    public interface IReplicatable
    {
        Task<bool> Replicate(InboundEvent evt);
    }
}
