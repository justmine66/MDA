using Disruptor;
using MDA.EventSourcing;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个出站事件处理器。
    /// </summary>
    public interface IOutboundDomainEventHandler<in T> : IEventHandler<T>
        where T : IDomainEvent, new()
    {

    }
}
