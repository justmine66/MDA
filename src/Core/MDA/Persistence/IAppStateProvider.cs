using System.Threading.Tasks;
using MDA.EventSourcing;

namespace MDA.Persistence
{
    public interface IAppStateProvider
    {
        Task<T> GetAsync<T>(string principal) where T : EventSourcedRootEntity;
    }
}
