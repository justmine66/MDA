using MDA.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TrackingShips.Application.Commands;
using TrackingShips.Domain.Model;
using TrackingShips.Port.Adapters;

namespace TrackingShips.Application
{
    /// <summary>
    /// 跟踪应用服务
    /// </summary>
    public class TrackingAppService
    {
        private readonly IServiceProvider _provider;

        public TrackingAppService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ArrivalSetsShipsLocationAsync(ShipArrivedCommand command)
        {
            var domainEventPublisher = _provider.GetService<IInboundDisruptor<ShipArrived>>();
            await domainEventPublisher.PublishInboundEventAsync<ShipArrivedMapper, ShipArrivedCommand>(typeof(Ship).FullName, command);
        }

        public void DeparturePutsShipOutToSea(string port)
        {

        }
    }
}
