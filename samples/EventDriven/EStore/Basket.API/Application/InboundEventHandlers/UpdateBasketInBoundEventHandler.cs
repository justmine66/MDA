using Basket.API.Port.Adapters.Inbound;
using MDA.Eventing;

namespace Basket.API.Application.InboundEventHandlers
{
    public class UpdateBasketInboundEventHandler : IInBoundEventHandler<UpdateBasketInboundEvent>
    {
        public void OnEvent(UpdateBasketInboundEvent @event, long sequence, bool endOfBatch)
        {

        }
    }
}
