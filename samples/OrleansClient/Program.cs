using System;
using System.Linq;
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
            var output = "123,1231,3123.2,52342".SplitWithSpan(',').ToArray();

            //var client = new ClientBuilder()
            //  .UseLocalhostClustering()
            //  .Configure<ClusterOptions>(options =>
            //  {
            //      options.ClusterId = "samples";
            //      options.ServiceId = "OrleansSiloHost";
            //  })
            //  .ConfigureLogging(logging => logging.AddConsole())
            //  .Build();

            //await client.Connect();

            //var friend = client.GetGrain<IHello>(0);
            //var response = await friend.SayHelloAsync("Good morning, my friend!");
            //Console.WriteLine("\n\n{0}\n\n", response);

            Console.Read();
        }
    }
}
