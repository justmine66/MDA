using MDA.Core.EventSourcing;
using MDA.Shared;

namespace Basket.Domain.Aggregates.BasketAggregate
{
    public class BasketItemId : StringIdentity
    {
        public BasketItemId(string id)
            : base(id)
        {
            Assert.NotNullOrEmpty(id, nameof(id));
        }
    }
}
