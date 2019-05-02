using MDA.EventSourcing;

namespace TrackingShips.Domain.Model
{
    public class ShipArrived : DomainEvent
    {
        public string Port { get; set; }
    }
}
