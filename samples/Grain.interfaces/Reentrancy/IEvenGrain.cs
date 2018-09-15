using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Reentrancy
{
    public interface IEvenGrain : IGrainWithIntegerKey
    {
        Task<bool> IsEven(int num);
    }
}
