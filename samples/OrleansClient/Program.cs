using Grain.interfaces.ActorCollection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using OrleansClient.Concurrency;
using System;
using System.Threading.Tasks;

namespace OrleansClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //unsafe
            //{
            //    var num = SpanDemo.Test5();
            //    Console.WriteLine(*num);

            //    Console.WriteLine(*num);
            //}

            //MemoryDemo.GenerateIdSpan1();
            //var output = "123,1231,3123.2,52342".SplitWithSpan(',').ToArray();

            var client = new ClientBuilder()
              .UseLocalhostClustering()
              .Configure<ClusterOptions>(options =>
              {
                  options.ClusterId = "samples";
                  options.ServiceId = "OrleansSiloHost";
              })
              .ConfigureLogging(logging => logging.AddConsole())
              .Build();

            await client.Connect();

            //var friend = client.GetGrain<IHello>(0);
            //var response = await friend.SayHelloAsync("Good morning, my friend!");
            //Console.WriteLine("\n\n{0}\n\n", response);

            //var hello = client.GetGrain<IExampleGrain>(0, "a string!", null);
            //await hello.Hello();

            //var e0 = client.GetGrain<IEmployee>(Guid.NewGuid());
            //var e1 = client.GetGrain<IEmployee>(Guid.NewGuid());
            //var e2 = client.GetGrain<IEmployee>(Guid.NewGuid());
            //var e3 = client.GetGrain<IEmployee>(Guid.NewGuid());
            //var e4 = client.GetGrain<IEmployee>(Guid.NewGuid());

            //var m0 = client.GetGrain<IManager>(Guid.NewGuid());
            //var m1 = client.GetGrain<IManager>(Guid.NewGuid());
            //var m0e = m0.AsEmployee().Result;
            //var m1e = m1.AsEmployee().Result;

            //await m0e.Promote(10);
            //await m1e.Promote(11);

            //m0.AddDirectReport(e0).Wait();
            //m0.AddDirectReport(e1).Wait();
            //m0.AddDirectReport(e2).Wait();

            //m1.AddDirectReport(m0e).Wait();
            //m1.AddDirectReport(e3).Wait();

            //m1.AddDirectReport(e4).Wait();

            //await ConcurrencyTest.Run(client);
            //await StockClient.Run(client);

            //await ReentrancyClient.RunSlow(client);
            //await ReentrancyClient.RunFast(client);
            await ReentrancyClient.RunIsEven(client);

            Console.Read();
        }
    }
}
