using Grain.interfaces.DeclarativePersistence;
using Orleans.Providers;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.DeclarativePersistence
{
    [StorageProvider(ProviderName = "MemoryStore")]
    public class Employee : Orleans.Grain<EmployeeState>, IEmployee
    {
        public Task<int> GetLevel()
        {
            throw new NotImplementedException();
        }

        public Task<IManager> GetManager()
        {
            throw new NotImplementedException();
        }

        public Task Greeting(GreetingData data)
        {
            throw new NotImplementedException();
        }

        public Task Promote(int newLevel)
        {
            State.Level = newLevel;
            return base.WriteStateAsync();
        }

        public Task SetManager(IManager manager)
        {
            State.Manager = manager;
            return base.WriteStateAsync();
        }
    }
}
