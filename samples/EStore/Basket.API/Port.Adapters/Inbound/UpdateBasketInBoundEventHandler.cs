using MDA.Eventing;
using System;

namespace Basket.API.Port.Adapters.Inbound
{
    public class UpdateBasketInBoundEventHandler : IInBoundEventHandler<UpdateBasketInboundEvent>
    {
        public void OnEvent(UpdateBasketInboundEvent @event, long sequence, bool endOfBatch)
        {
            throw new NotImplementedException();
        }
    }
}
