using Grain.interfaces.Concurrency;
using Orleans;
using System.Threading.Tasks;

namespace Grain.Implementations.Concurrency
{
    public class Manager : Orleans.Grain, IManager
    {
        public async Task AddDirectReport(IEmployee employee)
        {
            await employee.Greeting(new GreetingData
            {
                From = this.GetPrimaryKey(),
                Message = "Welcome to my team!"
            });
        }
    }
}
