using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using System.Net;
using System.Threading.Tasks;

namespace OrleansSiloHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "HelloWorldApp";
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();

            await host.StartAsync();
        }
    }
}
