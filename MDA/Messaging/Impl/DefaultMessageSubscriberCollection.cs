using System.Collections;
using System.Collections.Generic;

namespace MDA.Messaging.Impl
{
    public class DefaultMessageSubscriberCollection : IMessageSubscriberCollection
    {
        public IEnumerator<MessageSubscriberDescriptor> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(MessageSubscriberDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(MessageSubscriberDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(MessageSubscriberDescriptor[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(MessageSubscriberDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public int IndexOf(MessageSubscriberDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, MessageSubscriberDescriptor item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public MessageSubscriberDescriptor this[int index]
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
    }
}
