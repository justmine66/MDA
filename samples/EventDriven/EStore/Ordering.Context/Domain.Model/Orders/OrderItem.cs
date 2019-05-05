using MDA.EventSourcing;
using Ordering.Context.Exceptions;

namespace Ordering.Context.Domain.Model.Orders
{
    public class OrderItem : Entity<OrderItemId>
    {
        public OrderItem(
            OrderItemId id,
            int productId,
            string productName,
            string pictureUrl,
            decimal unitPrice,
            decimal discount,
            int units) : base(id)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            UnitPrice = unitPrice;
            Discount = discount;
            Units = units;
        }

        public int ProductId { get; }
        public string ProductName { get; }
        public string PictureUrl { get; }
        public decimal UnitPrice { get; }
        public decimal Discount { get; private set; }
        public int Units { get; private set; }

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new OrderingDomainException("Discount is not valid");
            }

            Discount = discount;
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}
