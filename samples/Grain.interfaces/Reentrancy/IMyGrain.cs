using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Reentrancy
{
    public interface IMyGrain : IGrainWithGuidKey
    {
        Task Process(object payload);
    }
}
