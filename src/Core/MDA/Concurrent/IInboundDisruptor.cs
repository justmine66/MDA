using Disruptor;
using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<T> where T : InboundEvent
    {
        Task<bool> PublishInboundEventAsync<TKey,TMessage>(IEventTranslatorTwoArg<T, TKey, TMessage> translator,
            TKey messageKey, TMessage message);

        Task<bool> PublishInboundEventAsync<TMessage>(IEventTranslatorOneArg<T, TMessage> translator, TMessage message);

        Task<bool> PublishInboundEventAsync<TMessage>(IEventTranslator<T> translator);
    }
}
