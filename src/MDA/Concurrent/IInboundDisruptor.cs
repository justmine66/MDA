using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor
    {
        Task<bool> SendAsync<T>(T evt) where T : InboundEvent, new();
    }
}
