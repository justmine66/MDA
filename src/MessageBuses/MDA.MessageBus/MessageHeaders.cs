using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MDA.MessageBus
{
    public class MessageHeaders : IEnumerable<MessageHeader>, IList<MessageHeader>
    {
        private readonly List<MessageHeader> _headers = new List<MessageHeader>();

        public void Add(string key, byte[] value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            _headers.Add(new MessageHeader(key, value));
        }

        public void Add(MessageHeader header) => _headers.Add(header);

        public void Clear() => _headers.Clear();

        public bool HasKey(string key) => _headers.Select(it => it.Key).Contains(key);

        public bool Contains(MessageHeader header) => _headers.Contains(header);

        public void CopyTo(MessageHeader[] array, int arrayIndex) => _headers.CopyTo(array, arrayIndex);

        public bool Remove(MessageHeader header) => _headers.Remove(header);

        public int Count => _headers.Count;

        public bool IsReadOnly => false;

        public IEnumerator<MessageHeader> GetEnumerator() => _headers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(MessageHeader header)
            => _headers.IndexOf(header);

        public void Insert(int index, MessageHeader header)
            => _headers.Insert(index, header);

        public void RemoveAt(int index) => _headers.RemoveAt(index);

        public MessageHeader this[int index]
        {
            get => _headers[index];
            set => _headers[index] = value;
        }
    }
}
