using Grain.interfaces.Concurrency;
using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansClient.Concurrency
{
    public class ConcurrencyTest
    {
        public static async Task Run(IClusterClient client)
        {
            var e0 = client.GetGrain<IEmployee>(Guid.NewGuid());
            var m0 = client.GetGrain<IManager>(Guid.NewGuid());

            m0.AddDirectReport(e0).Wait();
        }
    }
}
