using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Persistence
{
    public interface IMyPersistenceGrain : IGrainWithGuidKey
    {
        Task<int> DoRead();
        Task DoWrite(int val);
    }
}
