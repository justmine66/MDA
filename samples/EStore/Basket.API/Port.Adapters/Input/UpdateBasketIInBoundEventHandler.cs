using MDA.Eventing;
using System;

namespace Basket.API.Port.Adapters.Input
{
    public class UpdateBasketIInBoundEventHandler : IInBoundEventHandler<UpdateBasketInboundEvent>
    {
        public void OnEvent(UpdateBasketInboundEvent @event, long sequence, bool endOfBatch)
        {
            throw new NotImplementedException();
        }
    }
}
