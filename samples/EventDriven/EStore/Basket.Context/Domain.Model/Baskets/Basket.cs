using System.Collections.Generic;
using MDA.EventSourcing;
using MDA.Shared;

namespace Basket.Context.Domain.Model.Baskets
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

            Apply(new BasketCreated(buyerId, items));
        }

        public void Modify(string buyerId, List<BasketItem> items)
        {
            Assert.NotNullOrEmpty(nameof(buyerId), buyerId);
            Assert.NotNull(nameof(items), items);

            Apply(new BasketModified(buyerId, items));
        }

        public string BuyerId { get; private set; }
        public List<BasketItem> Items { get; private set; }

        private void OnDomainEvent(BasketCreated e)
        {
            BuyerId = e.BuyerId;
            Items = e.Items;
        }

        private void OnDomainEvent(BasketModified e)
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
