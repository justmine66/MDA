using System.Collections;
using System.Collections.Generic;

namespace MDA.Messaging.Impl
{
    public class DefaultMessageSubscriberCollection : IMessageSubscriberCollection
    {
        private readonly List<MessageSubscriberDescriptor> _descriptors;

        public DefaultMessageSubscriberCollection()
        {
            _descriptors = new List<MessageSubscriberDescriptor>();
        }

        public IEnumerator<MessageSubscriberDescriptor> GetEnumerator()
        {
            return _descriptors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(MessageSubscriberDescriptor item)
        {
           _descriptors.Add(item);
        }

        public void Clear()
        {
            _descriptors.Clear();
        }

        public bool Contains(MessageSubscriberDescriptor item)
        {
            return _descriptors.Contains(item);
        }

        public void CopyTo(MessageSubscriberDescriptor[] array, int arrayIndex)
        {
            _descriptors.CopyTo(array, arrayIndex);
        }

        public bool Remove(MessageSubscriberDescriptor item)
        {
            return _descriptors.Remove(item);
        }

        public int Count => _descriptors.Count;
        public bool IsReadOnly => false;
        public int IndexOf(MessageSubscriberDescriptor item)
        {
            return _descriptors.IndexOf(item);
        }

        public void Insert(int index, MessageSubscriberDescriptor item)
        {
            _descriptors.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _descriptors.RemoveAt(index);
        }

        public MessageSubscriberDescriptor this[int index]
        {
            get => _descriptors[index];
            set => _descriptors[index] = value;
        }
    }
}
