using System.Collections.Generic;
using MDA.EventSourcing;

namespace Basket.Context.Domain.Model.Baskets
{
    public class BasketCreated : DomainEvent
    {
        public BasketCreated(string buyerId, List<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        public string BuyerId { get; }
        public List<BasketItem> Items { get; }
    }
}
