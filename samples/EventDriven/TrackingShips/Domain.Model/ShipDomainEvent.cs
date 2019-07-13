using System;
using MDA.EventSourcing;

namespace TrackingShips.Domain.Model
{
    public class ShipDomainEvent : DomainEvent
    {
        public ShipDomainEvent()
        {
            Principal = new BusinessPrincipal()
            {
                Id = Guid.NewGuid().ToString(),
                TypeName = typeof(Ship).FullName
            };
        }
    }
}
