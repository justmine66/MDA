using System.Collections.Generic;
using MDA.EventSourcing;

namespace Basket.Context.Domain.Model.Baskets
{
    public class BasketModified : DomainEvent
    {
        public BasketModified(string buyerId, List<BasketItem> items)
        {
            BuyerId = buyerId;
            Items = items;
        }

        public string BuyerId { get; }
        public List<BasketItem> Items { get; }
    }
}
