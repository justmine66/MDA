using System.Threading.Tasks;
using MDA.Eventing;
using MDA.Persistence;
using Microsoft.Extensions.Logging;
using TrackingShips.Domain.Model;

namespace TrackingShips.Application.DomainEventHandlers
{
    public class ShipArrivedDomainEventHandler : InBoundDomainEventHandler<Ship, ShipArrived>
    {
        private readonly ILogger _logger;
        public ShipArrivedDomainEventHandler(IAppStateProvider context, ILogger<ShipArrivedDomainEventHandler> logger)
            : base(context)
        {
            _logger = logger;
        }

        public override Task OnEventAsync(Ship principal)
        {
            _logger.LogInformation($"The ship{principal.Name} arrived port[{principal.Location}].");
            return Task.CompletedTask;
        }
    }
}
