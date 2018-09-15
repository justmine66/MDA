using Grain.interfaces.Reentrancy;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Reentrancy
{
    public class EvenGrain : Orleans.Grain, IEvenGrain
    {
        public async Task<bool> IsEven(int num)
        {
            if (num == 0) return true;
            var oddGrain = this.GrainFactory.GetGrain<IOddGrain>(0);
            return await oddGrain.IsOdd(num - 1);
        }
    }
}
