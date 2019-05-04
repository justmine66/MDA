using MDA;
using MDA.Eventing;
using MDA.EventStoring.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TrackingShips.Application;
using TrackingShips.Application.DomainEventHandlers;
using TrackingShips.Domain.Model;

namespace TrackingShips
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices(services => {
                    services.AddHostedService<Engine>();
                    services.AddMdaServices();
                    services.AddInMemoryEventStoringServices();
                    services.AddSingleton<TrackingAppService>();
                    services.AddTransient<IInBoundDomainEventHandler<ShipArrived>, ShipArrivedDomainEventHandler>();
                    services.AddTransient<IInBoundDomainEventHandler<ShipDeparted>, ShipDepartedDomainEventHandler>();
                })
                .ConfigureLogging(builder =>
                {
                    builder.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}
