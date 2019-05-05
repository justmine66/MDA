using System.Collections.Generic;
using MDA.EventSourcing;
using MDA.Shared;

namespace TrackingShips.Domain.Model
{
    /// <summary>
    /// 船
    /// </summary>
    public class Ship : EventSourcedRootEntity
    {
        public Ship(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {

        }

        public Ship(string name, string location)
        {
            Assert.NotNullOrEmpty(nameof(name), name);
            Assert.NotNullOrEmpty(nameof(location), location);

            Name = name;
            Location = location;
        }

        /// <summary>
        /// 到达港口
        /// </summary>
        /// <param name="port">港口</param>
        public void ArrivedAt(string port)
        {
            Assert.NotNullOrEmpty(nameof(port), port);

            var e = new ShipArrived() { Ship = Name, Port = port };
            Apply(e);
        }

        /// <summary>
        /// 离开港口
        /// </summary>
        /// <param name="port">港口</param>
        public void DepartedAt(string port)
        {
            Assert.NotNullOrEmpty(nameof(port), port);

            var e = new ShipDeparted() { Ship = Name, Port = port };
            Apply(e);
        }

        public void OnDomainEvent(ShipArrived e)
        {
            Name = e.Ship;
            Location = e.Port;
        }

        public void OnDomainEvent(ShipDeparted e)
        {
            Name = e.Ship;
            Location = e.Port;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; private set; }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return Name;
        }
    }
}
