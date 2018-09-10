using Grain.interfaces.ActorIdentity;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.ActorIdentity
{
    public class ExampleGrain : Orleans.Grain, IExampleGrain
    {
        public Task Hello()
        {
            string keyExtension;
            var primaryKey = this.GetPrimaryKeyLong(out keyExtension);

            Console.WriteLine($"Hello[primaryKey: {primaryKey}] from " + keyExtension);

            return Task.CompletedTask;
        }
    }
}
