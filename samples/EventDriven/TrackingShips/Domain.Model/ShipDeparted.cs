using MDA.EventSourcing;

namespace TrackingShips.Domain.Model
{
    public class ShipDeparted : DomainEvent
    {
        public string Port { get; set; }
    }
}
