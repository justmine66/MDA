using MDA.Eventing;
using MDA.Persistence;

namespace Basket.API.IntegrationEvents
{
    public class UpdateBasketJournaler : IInBoundEventHandler<UpdateBasketEvent>
    {
        private readonly IJournalable _journaler;

        public UpdateBasketJournaler(IJournalable journaler)
        {
            _journaler = journaler;
        }

        public void OnEvent(UpdateBasketEvent @event, long sequence, bool endOfBatch)
        {
            _journaler.LogToStorage(@event);
        }
    }
}
