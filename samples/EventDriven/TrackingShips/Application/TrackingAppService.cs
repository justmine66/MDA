using System;
using System.Threading.Tasks;
using MDA.Concurrent;
using MDA.EventSourcing;
using Microsoft.Extensions.DependencyInjection;
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
            var principal = new BusinessPrincipal() { Id = "ship_1", TypeName = typeof(Ship).FullName };
            await domainEventPublisher.PublishInboundEventAsync<ShipArrivedMapper, ShipArrivedCommand>(principal, command);
        }

        public async Task DeparturePutsShipOutToSea(ShipDepartedCommand command)
        {
            var domainEventPublisher = _provider.GetService<IInboundDisruptor<ShipDeparted>>();
            var principal = new BusinessPrincipal() { Id = "ship_2", TypeName = typeof(Ship).FullName };
            await domainEventPublisher.PublishInboundEventAsync<ShipDepartedMapper, ShipDepartedCommand>(principal, command);
        }
    }
}
