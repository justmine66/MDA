using Basket.API.Model;
using MDA.Eventing;
using System.Collections.Generic;

namespace Basket.API.IntegrationEvents
{
    public class UpdateBasketEvent : InboundEvent
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
