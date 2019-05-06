using MDA.Commanding;

namespace TrackingShips.Application.Commands
{
    public class ShipArrivedCommand : Command
    {
        public string Ship { get;  set; }
        public string Port { get;  set; }
    }
}
