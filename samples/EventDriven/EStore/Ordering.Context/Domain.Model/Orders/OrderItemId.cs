using MDA.EventSourcing;
using MDA.Shared;

namespace Ordering.Context.Domain.Model.Orders
{
    public class OrderItemId : StringIdentity
    {
        public OrderItemId(string id)
            : base(id)
        {
            Assert.NotNullOrEmpty(id, nameof(id));
        }
    }
}
