using MDA.EventSourcing;

namespace TrackingShips.Domain.Model
{
    public class ShipArrived : DomainEvent
    {
        public string Ship { get; set; }
        public string Port { get; set; }
    }
}
