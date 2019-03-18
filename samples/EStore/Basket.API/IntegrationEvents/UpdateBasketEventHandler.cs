using MDA.Eventing;
using System;

namespace Basket.API.IntegrationEvents
{
    public class UpdateBasketEventHandler : IInBoundEventHandler<UpdateBasketEvent>
    {
        public void OnEvent(UpdateBasketEvent @event, long sequence, bool endOfBatch)
        {
            throw new NotImplementedException();
        }
    }
}
