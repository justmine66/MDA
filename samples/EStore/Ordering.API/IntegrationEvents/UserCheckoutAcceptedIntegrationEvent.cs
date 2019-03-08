using MDA.MessageBus;

namespace Ordering.API.IntegrationEvents
{
    /// <summary>
    /// 下单事件
    /// </summary>
    public class UserCheckoutAcceptedIntegrationEvent : Message
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
