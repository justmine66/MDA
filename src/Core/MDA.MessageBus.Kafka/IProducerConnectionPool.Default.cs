using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MDA.MessageBus.Kafka
{
    public class DefaultProducerConnectionPool : IProducerConnectionPool, IDisposable
    {
        private readonly KafkaOptions _options;
        private readonly ConcurrentQueue<IProducer<string, byte[]>> _producerPool;

        private int _producerReferenceCount;
        private int _connectionPoolSize;
        private bool _disposed;

        public DefaultProducerConnectionPool(IOptions<KafkaOptions> options)
        {
            _options = options.Value;
            _connectionPoolSize = _options.ConnectionPoolSize;
            _producerPool = new ConcurrentQueue<IProducer<string, byte[]>>();
        }

        public string ServersAddress => _options.Servers;

        public IProducer<string, byte[]> RentProducer()
        {
            if (_producerPool.TryDequeue(out var producer))
            {
                Interlocked.Decrement(ref _producerReferenceCount);

                return producer;
            }

            // 三、设置应答和重试
            // 1. 应答：Broker向生产者发出的消息已收到的确认。
            // 涉及的参数：
            // 1.1 模式
            // 1.1.1 acks=0，无应答，消息一定会丢，吞吐最高。
            // 1.1.2 acks=1(默认)，Leader将消息写到本地日志后，向生产者应答，消息可能会丢，吞吐较高。
            // 1.1.3 acks=all，Leader将消息同步到所有的Follower后，向生产者应答，消息不会丢失，吞吐最低。
            _options.MainConfig["request.required.acks"] = "1";
            // 1.1 额定应答时长。
            _options.MainConfig["request.timeout.ms"] = "3000";
            // 2. 重试：超过额定应答时长，生产者尝试重新发送消息，即超时重传。
            // 注意：重试会破坏消费顺序、或消费重复消息，如需解决，请开启幂等性，一般不建议开启，为了保障至少被消费一次语义的性能最大化。
            _options.MainConfig["enable.idempotence"] = "false";
            _options.MainConfig["message.send.max.retries"] = "5";

            producer = new ProducerBuilder<string, byte[]>(_options.AsKafkaConfig())
                .Build();

            return producer;
        }

        public bool Return(IProducer<string, byte[]> producer)
        {
            if (Interlocked.Increment(ref _producerReferenceCount) <= _connectionPoolSize)
            {
                _producerPool.Enqueue(producer);

                return true;
            }

            Interlocked.Decrement(ref _producerReferenceCount);

            return false;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                //清理托管资源
                _connectionPoolSize = 0;

                while (_producerPool.TryDequeue(out var producer)) 
                    producer.Dispose();
            }

            //清理非托管资源

            _disposed = true;
        }
    }
}
