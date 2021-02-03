using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MDA.Infrastructure.DataStructures
{
    /// <summary>
    /// LinkedHashMap is a dictionary which preserve the order of inserting elements.
    /// It's similar to LinkedHashMap in Java.
    /// Could be used for LRU caches.
    /// </summary>
    public class LinkedHashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        public int Count => _valueByKey.Count;
        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => _valueByKey.Keys;
        public ICollection<TValue> Values => _valueByKey.Values.Select(v => v.Value).ToArray();

        private LinkedList<TKey> _items;
        private IDictionary<TKey, ValueNodePair> _valueByKey;

        public void Clear()
        {
            _items = new LinkedList<TKey>();
            _valueByKey = new Dictionary<TKey, ValueNodePair>();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _valueByKey.TryGetValue(item.Key, out var value) && value.Value.Equals(item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            return _valueByKey.ContainsKey(key);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (ContainsKey(key)) throw new ArgumentException("Key is already presented", nameof(key));
            var node = _items.AddLast(key);
            _valueByKey.Add(key, new ValueNodePair(value, node));
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (!_valueByKey.TryGetValue(key, out var tempValue)) return false;
            var node = tempValue.Node;
            _valueByKey.Remove(key);
            _items.Remove(node);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_valueByKey.TryGetValue(key, out var tempValue))
            {
                value = default;
                return false;
            }

            value = tempValue.Value;
            return true;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var maxLen = Math.Max(array.Length, arrayIndex + Count);
            using var enumerator = GetEnumerator();
            for (var i = arrayIndex; i < maxLen; i++)
            {
                array[i] = enumerator.Current;
                enumerator.MoveNext();
            }
        }

        public TValue this[TKey key]
        {
            get => _valueByKey[key].Value;
            set
            {
                if (!_valueByKey.ContainsKey(key))
                {
                    Add(key, value);
                }
                else
                {
                    _valueByKey[key].Value = value;
                    UpdateKey(key);
                }
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            var node = _items.First;
            while (node != null)
            {
                yield return new KeyValuePair<TKey, TValue>(node.Value, _valueByKey[node.Value].Value);
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void UpdateKey(TKey key)
        {
            if (!_valueByKey.ContainsKey(key)) return;
            var node = _valueByKey[key].Node;
            if (node.Next == null) return;

            _items.Remove(node);
            _items.AddLast(node);
        }

        private void Init()
        {
            Clear();
        }

        private class ValueNodePair
        {
            public TValue Value { get; set; }
            public LinkedListNode<TKey> Node { get; }

            public ValueNodePair(TValue value, LinkedListNode<TKey> node)
            {
                Value = value;
                Node = node;
            }
        }

        public LinkedHashMap()
        {
            Init();
        }
    }
}
