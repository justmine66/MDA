using MDA.MessageBus;

namespace MDA.Domain.Commands
{
    public static class MessageSubscriberManagerExtensions
    {
        public static IMessageSubscriberManager SubscribeDomainCommands(this IMessageSubscriberManager subscriber)
        {
            subscriber.Subscribe<DomainCommandTransportMessage, IMessageHandler<DomainCommandTransportMessage>>();
            subscriber.SubscribeAsync<DomainCommandTransportMessage, IAsyncMessageHandler<DomainCommandTransportMessage>>();

            return subscriber;
        }
    }
}
