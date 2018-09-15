using Grain.interfaces.Reentrancy;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Reentrancy
{
    public class SlowpokeGrain : Orleans.Grain, ISlowpokeGrain
    {
        public async Task GoSlow()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        public async Task GoFast()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
}
