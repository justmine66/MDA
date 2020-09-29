using MDA.MessageBus;

namespace MDA.Domain.Commands
{
    public static class MessageBusExtensions
    {
        public static IMessageSubscriber SubscribeDomainCommands(this IMessageSubscriber subscriber)
        {
            subscriber.Subscribe<DomainCommandTransportMessage, IMessageHandler<DomainCommandTransportMessage>>();
            subscriber.SubscribeAsync<DomainCommandTransportMessage, IAsyncMessageHandler<DomainCommandTransportMessage>>();

            return subscriber;
        }
    }
}
