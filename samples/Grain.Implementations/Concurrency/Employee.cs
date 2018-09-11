using Grain.interfaces.Concurrency;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Concurrency
{
    public class Employee : Orleans.Grain, IEmployee
    {
        public async Task Greeting(GreetingData data)
        {
            Console.WriteLine("{0} said: {1}", data.From, data.Message);

            if (data.Count >= 3) return;

            var fromGrain = this.GrainFactory.GetGrain<IEmployee>(data.From);
            await fromGrain.Greeting(new GreetingData
            {
                From = this.GetPrimaryKey(),
                Message = "Thanks!", 
                Count = data.Count + 1
            });
        }
    }
}
