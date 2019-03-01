using System;

namespace MDA.MessageBus
{
    /// <summary>
    /// 慢消息感知接口
    /// </summary>
    public interface ISlowMessageAware
    {
        event EventHandler<SlowMessageEventArgs> OnSlowMessage;
    }
}
