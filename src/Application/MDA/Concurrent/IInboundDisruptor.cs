using Disruptor;
using MDA.Eventing;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<T> where T : InboundEvent
    {
        Task<bool> PublishInboundEventAsync<TMessage>(IEventTranslatorTwoArg<T, string, TMessage> translator,
            string messageKey, TMessage message);
    }
}
