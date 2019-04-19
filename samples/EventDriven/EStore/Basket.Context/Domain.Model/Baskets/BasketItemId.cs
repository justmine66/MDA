using MDA.EventSourcing;
using MDA.Shared;

namespace Basket.Context.Domain.Model.Baskets
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
