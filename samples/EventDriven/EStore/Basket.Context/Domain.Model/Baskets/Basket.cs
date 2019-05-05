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

        private Basket()
        {
            _items = new List<BasketItem>();
        }

        public Basket(string buyerId, BasketItem item)
            : this()
        {
            Assert.NotNullOrEmpty(nameof(buyerId), buyerId);
            Assert.NotNull(nameof(item), item);

            Apply(new BasketCreated(buyerId, item));
        }

        public void Modify(string buyerId, List<BasketItem> items)
        {
            Assert.NotNullOrEmpty(nameof(buyerId), buyerId);
            Assert.NotNull(nameof(items), items);

            Apply(new BasketModified(buyerId, items));
        }

        public string BuyerId { get; private set; }
        public List<BasketItem> _items { get; private set; }
        public IEnumerable<BasketItem> Items => _items;

        private void OnDomainEvent(BasketCreated e)
        {
            BuyerId = e.BuyerId;
            _items.Add(e.Item);
        }

        private void OnDomainEvent(BasketModified e)
        {
            BuyerId = e.BuyerId;
            _items = e.Items;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return BuyerId;
        }
    }
}
