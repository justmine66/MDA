using MDA.MessageBus;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个出站事件，将被发送到网线（事件总线等）。
    /// </summary>
    public abstract class OutboundEvent : SequenceMessage
    {

    }
}
