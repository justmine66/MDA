using MDA.Eventing;
using TrackingShips.Application.Commands;
using TrackingShips.Domain.Model;

namespace TrackingShips.Port.Adapters
{
    public class ShipDepartedMapper : EventTranslatorTwoArg<ShipDeparted, ShipDepartedCommand>
    {
        public override void MapDomainEvent(ShipDeparted domainEvent, ShipDepartedCommand command)
        {
            domainEvent.Ship = command.Ship;
            domainEvent.Port = command.Port;
        }
    }
}
