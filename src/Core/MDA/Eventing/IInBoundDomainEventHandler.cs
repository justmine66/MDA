using Disruptor;
using MDA.EventSourcing;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个入站领域事件处理器。
    /// </summary>
    public interface IInBoundDomainEventHandler<in T> : IEventHandler<T> 
        where T : IDomainEvent, new()
    {

    }
}
