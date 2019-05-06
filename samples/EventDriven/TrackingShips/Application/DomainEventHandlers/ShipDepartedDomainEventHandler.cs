using System.Threading.Tasks;
using MDA.Eventing;
using MDA.Persistence;
using Microsoft.Extensions.Logging;
using TrackingShips.Domain.Model;

namespace TrackingShips.Application.DomainEventHandlers
{
    public class ShipDepartedDomainEventHandler : InBoundDomainEventHandler<Ship, ShipDeparted>
    {
        private readonly ILogger _logger;
        public ShipDepartedDomainEventHandler(IAppStateProvider context, ILogger<ShipDepartedDomainEventHandler> logger)
            : base(context)
        {
            _logger = logger;
        }

        public override Task OnEventAsync(Ship principal)
        {
            _logger.LogInformation($"The ship{principal.Name} departed port[{principal.Location}].");
            return Task.CompletedTask;
        }
    }
}
