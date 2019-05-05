using MDA.EventSourcing;

namespace Basket.Context.Domain.Model.Baskets
{
    public class BasketCreated : DomainEvent
    {
        public BasketCreated(string buyerId, BasketItem item)
        {
            BuyerId = buyerId;
            Item = item;
        }

        public string BuyerId { get; }
        public BasketItem Item { get; }
    }
}
