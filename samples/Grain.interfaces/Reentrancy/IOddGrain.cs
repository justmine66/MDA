using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Reentrancy
{
    public interface IOddGrain : IGrainWithIntegerKey
    {
        Task<bool> IsOdd(int num);
    }
}
