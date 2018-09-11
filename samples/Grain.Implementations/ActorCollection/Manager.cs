using Grain.interfaces.ActorCollection;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Grain.Implementations.ActorCollection
{
    public class Manager : Orleans.Grain, IManager
    {
        private IEmployee _me;
        private List<IEmployee> _reports = new List<IEmployee>();

        public override Task OnActivateAsync()
        {
            _me = this.GrainFactory.GetGrain<IEmployee>(this.GetPrimaryKey());

            return base.OnActivateAsync();
        }

        public async Task AddDirectReport(IEmployee employee)
        {
            _reports.Add(employee);
            await employee.SetManager(this);
            await employee.Greeting(_me, "Welcome to my team!");
        }

        public Task<IEmployee> AsEmployee()
        {
            return Task.FromResult(_me);
        }

        public Task<List<IEmployee>> GetDirectReports()
        {
            return Task.FromResult(_reports);
        }
    }
}
