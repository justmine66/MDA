using MDA.EventSourcing;

namespace Ordering.Context.Domain.Model.Orders
{
    public class OrderStatus: EnumerableValueObject
    {
        public static OrderStatus Submitted = new OrderStatus(1, nameof(Submitted));
        public static OrderStatus AwaitingValidation = new OrderStatus(2, nameof(AwaitingValidation));
        public static OrderStatus StockConfirmed = new OrderStatus(3, nameof(StockConfirmed));
        public static OrderStatus Paid = new OrderStatus(4, nameof(Paid));
        public static OrderStatus Shipped = new OrderStatus(5, nameof(Shipped));
        public static OrderStatus Cancelled = new OrderStatus(6, nameof(Cancelled));

        public OrderStatus(int id, string name) 
            : base(id, name)
        {
        }
    }
}
