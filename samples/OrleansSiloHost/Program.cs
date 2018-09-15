using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
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
                    options.ClusterId = "samples";
                    options.ServiceId = "OrleansSiloHost";
                })
                .AddAdoNetGrainStorage("MySqlStorage", options =>
                {
                    options.Invariant = "System.Data.MySqlClient";
                    options.ConnectionString = "server=47.75.161.43;port=3306;user id=root;database=mda;password=youngangel.c0m;characterset=utf8;sslmode=none;";
                    options.UseJsonFormat = true;
                })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();

            await host.StartAsync();

            Console.Read();
        }
    }
}
