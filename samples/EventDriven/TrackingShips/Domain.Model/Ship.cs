using MDA.EventSourcing;
using MDA.Shared;
using System.Collections.Generic;

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

        /// <summary>
        /// 到达港口
        /// </summary>
        /// <param name="port">港口</param>
        public void ArrivedAt(string port)
        {
            Assert.NotNullOrEmpty(nameof(port), port);

            var e = new ShipArrived() { Port = port };
            Apply(e);
        }

        /// <summary>
        /// 离开港口
        /// </summary>
        /// <param name="port">港口</param>
        public void DeparturedAt(string port)
        {
            Assert.NotNullOrEmpty(nameof(port), port);

            var e = new ShipDeparted() { Port = port };
            Apply(e);
        }

        protected void OnDomainEvent(ShipArrived e)
        {
            Location = e.Port;
        }

        protected void OnDomainEvent(ShipDeparted e)
        {
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
