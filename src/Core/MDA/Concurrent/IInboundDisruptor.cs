using Disruptor;
using MDA.EventSourcing;
using System.Threading.Tasks;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<TDomainEvent>
        where TDomainEvent : DomainEvent
    {
        Task<bool> PublishInboundEventAsync<TTranslator, TCommand>(string principal, TCommand message)
            where TTranslator : IEventTranslatorTwoArg<TDomainEvent, string, TCommand>, new();
    }
}
