using Basket.API.Model;
using Disruptor;
using MDA.Eventing;
using System.Collections.Generic;

namespace Basket.API.Port.Adapters.Inbound
{
    public class UpdateBasketInboundEvent : InboundEvent
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }

    public class UpdateBasketEventTranslator : IEventTranslatorTwoArg<UpdateBasketInboundEvent, string, CustomerBasket>
    {
        public void TranslateTo(UpdateBasketInboundEvent @event, long sequence, string requestId, CustomerBasket arg)
        {
            @event.Id = requestId;
            @event.Sequence = sequence;
            @event.BuyerId = arg.BuyerId;
            @event.Items = arg.Items;
        }
    }
}
