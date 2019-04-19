using MDA.EventSourcing;
using MDA.Shared;

namespace Basket.Context.Domain.Model.Baskets
{
    public class BasketItem : Entity<BasketItemId>
    {
        public BasketItem(
            BasketItemId id,
            string productId,
            string productName,
            decimal unitPrice,
            int quantity,
            string pictureUrl) : base(id)
        {
            Assert.NotNull(nameof(id), id);
            Assert.NotNullOrEmpty(nameof(productId), productId);
            Assert.NotNullOrEmpty(nameof(productName), productName);
            Assert.LengthGreaterThan(nameof(unitPrice), unitPrice, decimal.Zero);
            Assert.LengthGreaterThan(nameof(quantity), quantity, 0);
            Assert.NotNullOrEmpty(nameof(pictureUrl), pictureUrl);

            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            Quantity = quantity;
            PictureUrl = pictureUrl;
        }

        public string ProductId { get; }
        public string ProductName { get; }
        public decimal UnitPrice { get; }
        public int Quantity { get; }
        public string PictureUrl { get; }
    }
}
