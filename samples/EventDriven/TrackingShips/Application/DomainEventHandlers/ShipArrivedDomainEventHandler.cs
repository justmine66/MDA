using MDA.Eventing;
using MDA.Persistence;
using TrackingShips.Domain.Model;

namespace TrackingShips.Application.DomainEventHandlers
{
    public class ShipArrivedDomainEventHandler : IInBoundDomainEventHandler<ShipArrived>
    {
        private readonly IAppStateProvider _state;

        public ShipArrivedDomainEventHandler(IAppStateProvider stateProvider)
        {
            _state = stateProvider;
        }

        public void OnEvent(ShipArrived domainEvent, long sequence, bool endOfBatch)
        {
            var ship = _state.Get<Ship>(domainEvent.Principal);
            ship.OnDomainEvent(domainEvent);
        }
    }
}
