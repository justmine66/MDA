using MDA.Core.EventSourcing;
using MDA.Shared;

namespace Basket.Domain.Aggregates.BasketAggregate
{
    public class BasketItem : Entity
    {
        public BasketItem(
            BasketItemId id,
            string productId,
            string productName,
            decimal unitPrice,
            int quantity,
            string pictureUrl)
        {
            Assert.NotNull(nameof(id), id);
            Assert.NotNullOrEmpty(nameof(productId), productId);
            Assert.NotNullOrEmpty(nameof(productName), productName);
            Assert.LengthGreaterThan(nameof(unitPrice), unitPrice, decimal.Zero);
            Assert.LengthGreaterThan(nameof(quantity), quantity, 0);
            Assert.NotNullOrEmpty(nameof(pictureUrl), pictureUrl);

            Id = id;
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl;
        }

        public BasketItemId Id { get; }
        public string ProductId { get; }
        public string ProductName { get; }
        public decimal UnitPrice { get; }
        public int Quantity { get; }
        public string PictureUrl { get; }
    }
}
