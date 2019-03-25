using Basket.API.Model;
using MDA.Eventing;
using System.Collections.Generic;

namespace Basket.API.Port.Adapters.Input
{
    public class UpdateBasketInboundEvent : InboundEvent
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
