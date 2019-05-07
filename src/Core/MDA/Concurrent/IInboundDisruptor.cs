using Disruptor;
using MDA.EventSourcing;
using System.Threading.Tasks;
using MDA.Commanding;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<TDomainEvent>
        where TDomainEvent : DomainEvent, new()
    {
        Task<bool> PublishInboundEventAsync<TTranslator, TCommand>(BusinessPrincipal principal, TCommand command)
            where TCommand : ICommand
            where TTranslator : IEventTranslatorTwoArg<TDomainEvent, BusinessPrincipal, TCommand>, new();
    }
}
