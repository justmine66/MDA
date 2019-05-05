using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
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
            var command = new ShipArrivedCommand() { Ship = "泰坦尼克号", Port = "珍珠港" };
            await _appService.ArrivalSetsShipsLocationAsync(command);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
