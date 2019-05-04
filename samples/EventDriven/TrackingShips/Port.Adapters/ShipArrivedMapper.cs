using MDA.Eventing;
using TrackingShips.Application.Commands;
using TrackingShips.Domain.Model;

namespace TrackingShips.Port.Adapters
{
    public class ShipArrivedMapper : EventTranslatorTwoArg<ShipArrived, ShipArrivedCommand>
    {
        public override void MapDomainEvent(ShipArrived domainEvent, ShipArrivedCommand command)
        {
            domainEvent.Ship = command.Ship;
            domainEvent.Port = command.Port;
        }
    }
}
