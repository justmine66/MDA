using MDA.MessageBus;
using System;
using System.Threading.Tasks;

namespace Ordering.API.IntegrationEvents
{
    public class UserCheckoutAcceptedIntegrationEventHandler 
        : IMessageHandler<UserCheckoutAcceptedIntegrationEvent>
    {
        public Task HandleAsync(UserCheckoutAcceptedIntegrationEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
