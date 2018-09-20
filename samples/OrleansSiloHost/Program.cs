using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrleansSiloHost
{
    class Program
    {
        static readonly ManualResetEvent _siloStopped = new ManualResetEvent(false);

        static ISiloHost silo;
        static bool siloStopping = false;
        static readonly object syncLock = new object();

        static async Task Main(string[] args)
        {
            SetupApplicationShutdown();

            var host = CreateSilo();
            await host.StartAsync();

            _siloStopped.WaitOne();
        }

        static void SetupApplicationShutdown()
        {
            // Capture the user pressing Ctrl+C
            Console.CancelKeyPress += (s, a) =>
            {
                a.Cancel = true;
                lock (syncLock)
                {
                    if (!siloStopping)
                    {
                        siloStopping = true;
                        Task.Run(StopSilo).Ignore();
                    }
                }
            };
        }
        static ISiloHost CreateSilo()
        {
            var invariant = "MySql.Data.MySqlClient";
            var connectionString = "server=47.75.161.43;port=3306;user id=root;database=mda;password=youngangel.c0m;characterset=utf8;sslmode=none;";

            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "samples";
                    options.ServiceId = "OrleansSiloHost";
                })
                .UseAdoNetClustering(options =>
                {
                    options.Invariant = invariant;
                    options.ConnectionString = connectionString;
                })
                .UseAdoNetReminderService(options =>
                {
                    options.Invariant = invariant;
                    options.ConnectionString = connectionString;
                })
                .AddAdoNetGrainStorage("MySqlStorage", options =>
                {
                    options.Invariant = invariant;
                    options.ConnectionString = connectionString;
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.SiloPort = 11111;
                    options.GatewayPort = 30000;
                })
                .ConfigureLogging(logging =>
                {
                    logging
                    .SetMinimumLevel(LogLevel.Warning)
                    .AddConsole();
                });

            return builder.Build();
        }
        static async Task StopSilo()
        {
            await silo.StopAsync();
            _siloStopped.Set();
        }
    }
}
