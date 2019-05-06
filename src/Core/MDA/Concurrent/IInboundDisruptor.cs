using Disruptor;
using MDA.EventSourcing;
using System.Threading.Tasks;
using MDA.Commanding;

namespace MDA.Concurrent
{
    public interface IInboundDisruptor<TDomainEvent>
        where TDomainEvent : IDomainEvent, new()
    {
        Task<bool> PublishInboundEventAsync<TTranslator, TCommand>(string principal, TCommand command)
            where TCommand : ICommand
            where TTranslator : IEventTranslatorTwoArg<TDomainEvent, string, TCommand>, new();
    }
}
