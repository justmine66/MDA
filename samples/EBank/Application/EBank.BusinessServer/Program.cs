using EBank.BusinessServer.Bootstrap;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.BusinessServer
{
    class Program
    {
        private static readonly CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            Console.CancelKeyPress += ShutDown;

            await CreateHost(args).RunAsync(CancellationTokenSource.Token);
        }

        public static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(Bootstrapper.ConfigureServices)
                .Build();

        private static void ShutDown(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
        }
    }
}
