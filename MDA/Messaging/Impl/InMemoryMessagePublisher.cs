using System.Collections.Generic;
using System.Threading.Tasks;
using MDA.Common;

namespace MDA.Messaging.Impl
{
    /// <summary>
    /// 表示一个内存消息发布器。
    /// </summary>
    public class InMemoryMessagePublisher : IMessagePublisher
    {
        public Task<AsyncResult> PublishAllAsync<TMessage>(IEnumerable<TMessage> messages) where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }

        public Task<AsyncResult> PublishAsync<TMessage>(TMessage message) where TMessage : IMessage
        {
            throw new System.NotImplementedException();
        }
    }
}
