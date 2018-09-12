using Grain.interfaces.DeclarativePersistence;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grain.Implementations.DeclarativePersistence
{
    [StorageProvider(ProviderName = "MemoryStore")]
    public class Manager : Orleans.Grain<ManagerState>, IManager
    {
        public async Task AddDirectReport(IEmployee employee)
        {
            if (State.Reports == null)
            {
                State.Reports = new List<IEmployee>();
            }

            State.Reports.Add(employee);
            await employee.SetManager(this);

            var data = new GreetingData { From = this.GetPrimaryKey(), Message = "Welcome to my team!" };
            await employee.Greeting(data);
            Console.WriteLine("{0} said: {1}",
                        data.From.ToString(),
                        data.Message);
            
            await base.WriteStateAsync();
        }

        public Task<IEmployee> AsEmployee()
        {
            throw new NotImplementedException();
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            throw new NotImplementedException();
        }
    }
}
