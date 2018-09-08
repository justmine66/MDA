using System.Collections;
using System.Collections.Generic;

namespace MDA.Message.Abstractions
{
    /// <summary>
    /// 订阅者注册表.
    /// </summary>
    public interface IMessageSubscriberCollection : IList<MessageSubscriberDescriptor>, ICollection<MessageSubscriberDescriptor>, IEnumerable<MessageSubscriberDescriptor>, IEnumerable
    {
        IMessageSubscriberCollection New();
    }
}
