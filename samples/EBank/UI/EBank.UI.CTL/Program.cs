using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace EBank.UI.CTL
{
    public class Program
    {
        static async Task Main(string[] args) => await CreateHost(args).RunAsync();

        public static IHost CreateHost(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(Bootstrapper.ConfigureServices)
                .ConfigureContainer<ContainerBuilder>(Bootstrapper.ConfigureContainer)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .Build();
    }
}
