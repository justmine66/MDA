using System.Collections;
using System.Collections.Generic;

namespace MDA.Messaging
{
    /// <summary>
    /// A collection of message subsriber descriptor.
    /// </summary>
    public interface IMessageSubscriberCollection : IList<MessageSubscriberDescriptor>, ICollection<MessageSubscriberDescriptor>, IEnumerable<MessageSubscriberDescriptor>, IEnumerable
    {

    }
}
