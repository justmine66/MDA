using MDA.Eventing;
using TrackingShips.Domain.Model;

namespace TrackingShips.Application.DomainEventHandlers
{
    public class ShipDepartedDomainEventHandler : IInBoundDomainEventHandler<ShipDeparted>
    {
        public void OnEvent(ShipDeparted data, long sequence, bool endOfBatch)
        {
            throw new System.NotImplementedException();
        }
    }
}
