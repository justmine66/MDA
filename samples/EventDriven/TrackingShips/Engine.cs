using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using TrackingShips.Application;
using TrackingShips.Application.Commands;

namespace TrackingShips
{
    public class Engine : IHostedService
    {
        private readonly TrackingAppService _appService;
        public Engine(TrackingAppService appService)
        {
            _appService = appService;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            for (var i = 0; i < 10; i++)
            {
                var command1 = new ShipArrivedCommand() { Ship = "泰坦尼克号" + i, Port = "珍珠港" + i };
                await _appService.ArrivalSetsShipsLocationAsync(command1);

                var command2 = new ShipDepartedCommand() { Ship = "泰坦尼克号" + i, Port = "珍珠港" + i };
                await _appService.DeparturePutsShipOutToSea(command2);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
