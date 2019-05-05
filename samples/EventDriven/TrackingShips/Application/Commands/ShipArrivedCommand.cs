using MDA.Eventing;

namespace TrackingShips.Application.Commands
{
    public class ShipArrivedCommand 
    {
        public string Ship { get;  set; }
        public string Port { get;  set; }
    }
}
