using Disruptor;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个入站事件处理器。
    /// </summary>
    public interface IInBoundEventHandler<in T> : IEventHandler<T> 
        where T : InboundEvent, new()
    {
    }
}
