using System;
using System.Collections.Generic;

namespace MDA.Eventing
{
    /// <summary>
    /// 表示一个事件序列化器。
    /// </summary>
    public interface IEventSerializer
    {
        string Serialize(IDomainEvent domainEvent);
        string Serialize(IEnumerable<IDomainEvent> domainEvents);
        T Deserialize<T>(string serialization);
        object Deserialize<T>(string serialization, Type type);
    }
}
