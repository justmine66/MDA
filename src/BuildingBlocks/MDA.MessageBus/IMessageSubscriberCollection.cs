using System.Collections.Generic;

namespace MDA.MessageBus
{
    /// <summary>
    /// 订阅者集合.
    /// </summary>
    public interface IMessageSubscriberCollection : IList<MessageSubscriberDescriptor>
    {
        IMessageSubscriberCollection New();
    }
}
