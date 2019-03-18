using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<in T> where T : InboundEvent, new()
    {
        Task<bool> SendAsync(T evt);
    }
}
