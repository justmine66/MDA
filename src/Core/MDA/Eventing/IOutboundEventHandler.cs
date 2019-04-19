using Disruptor;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个出站事件处理器。
    /// </summary>
    public interface IOutboundEventHandler<in T> : IEventHandler<T>
        where T : OutboundEvent, new()
    {

    }
}
