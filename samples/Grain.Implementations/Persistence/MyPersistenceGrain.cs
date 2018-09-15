using Grain.interfaces.Persistence;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Grain.Implementations.Persistence
{
    [StorageProvider(ProviderName = "MySqlStorage")]
    public class MyPersistenceGrain : Grain<MyGrainState>, IMyPersistenceGrain
    {
        public async Task<int> DoRead()
        {
            await base.ReadStateAsync();
            return State.Field1;
        }

        public Task DoWrite(int val)
        {
            State.Field1 = val;
            return base.WriteStateAsync();
        }
    }
}
