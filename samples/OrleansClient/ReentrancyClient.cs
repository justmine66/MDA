using System;
using Grain.interfaces.Reentrancy;
using Orleans;
using System.Threading.Tasks;

namespace OrleansClient
{
    public class ReentrancyClient
    {
        public static async Task RunSlow(IClusterClient client)
        {
            var slowpoke = client.GetGrain<ISlowpokeGrain>(0);
            var start = DateTime.UtcNow;
            await Task.WhenAll(slowpoke.GoSlow(), slowpoke.GoSlow());
            Console.WriteLine($"{nameof(RunSlow)}耗时：" + DateTime.UtcNow.Subtract(start).TotalSeconds);
        }

        public static async Task RunFast(IClusterClient client)
        {
            var slowpoke = client.GetGrain<ISlowpokeGrain>(0);
            var start = DateTime.UtcNow;
            await Task.WhenAll(slowpoke.GoFast(), slowpoke.GoFast());
            Console.WriteLine($"{nameof(RunFast)}耗时：" + DateTime.UtcNow.Subtract(start).TotalSeconds);
        }

        public static async Task RunIsEven(IClusterClient client)
        {
            var evenGrain = client.GetGrain<IEvenGrain>(0);
            var isEven = await evenGrain.IsEven(2);

            Console.WriteLine("是否为偶数：" + isEven);
        }
    }
}
