using System.Collections.Generic;
using Basket.Domain.Events;
using MDA.Core.EventSourcing;
using MDA.Shared;

namespace Basket.Domain.Aggregates.BasketAggregate
{
    public class Basket : EventSourcedRootEntity
    {
        public Basket(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        public Basket(string buyerId, List<BasketItem> items)
        {
            Assert.NotNullOrEmpty(nameof(buyerId), buyerId);
            Assert.NotNull(nameof(items), items);

            Apply(new AddBasket(buyerId, items));
        }

        public void Modify(string buyerId, List<BasketItem> items)
        {
            Assert.NotNullOrEmpty(nameof(buyerId), buyerId);
            Assert.NotNull(nameof(items), items);

            Apply(new ModifyBasket(buyerId, items));
        }

        public string BuyerId { get; private set; }
        public List<BasketItem> Items { get; private set; }

        private void OnDomainEvent(AddBasket e)
        {
            BuyerId = e.BuyerId;
            Items = e.Items;
        }

        private void OnDomainEvent(ModifyBasket e)
        {
            BuyerId = e.BuyerId;
            Items = e.Items;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return BuyerId;
        }
    }
}
