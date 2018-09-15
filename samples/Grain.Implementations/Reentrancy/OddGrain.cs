using Grain.interfaces.Reentrancy;
using System.Threading.Tasks;

namespace Grain.Implementations.Reentrancy
{
    public class OddGrain : Orleans.Grain, IOddGrain
    {
        public async Task<bool> IsOdd(int num)
        {
            if (num == 0) return false;
            var evenGrain = this.GrainFactory.GetGrain<IEvenGrain>(0);
            return await evenGrain.IsEven(num - 1);
        }
    }
}
