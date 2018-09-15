using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Grain.interfaces.Reentrancy
{
    public interface ISlowpokeGrain : IGrainWithIntegerKey
    {
        Task GoSlow();

        [AlwaysInterleave]
        Task GoFast();
    }
}
