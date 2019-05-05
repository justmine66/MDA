using System.Collections.Generic;
using MDA.EventSourcing;

namespace Ordering.Context.Domain.Model.Orders
{
    public class Order : EventSourcedRootEntity
    {
        public Order(IEnumerable<IDomainEvent> eventStream, int streamVersion, string orderNo)
            : base(eventStream, streamVersion)
        {
            OrderNo = orderNo;
        }

        private Order()
        {
            _items = new List<OrderItem>();
        }

        protected Order(string orderNo, int buyerId, Address address, OrderStatus orderStatus, int paymentMethodId) : this()
        {
            BuyerId = buyerId;
            Address = address;
            OrderStatus = orderStatus;
            PaymentMethodId = paymentMethodId;
        }

        public string OrderNo { get; private set; }
        public Address Address { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public int BuyerId { get; private set; }
        public int PaymentMethodId { get; private set; }

        private readonly List<OrderItem> _items;
        public IEnumerable<OrderItem> OrderItems => _items;

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return OrderNo;
        }
    }
}
