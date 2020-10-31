using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MDA.MessageBus.Kafka
{
    public class DefaultKafkaProducerPool : IKafkaProducerPool, IDisposable
    {
        private readonly KafkaProducerOptions _options;
        private readonly ConcurrentQueue<IProducer<string, byte[]>> _producerPool;

        private int _producerReferenceCount;
        private int _connectionPoolSize;
        private bool _disposed;

        public DefaultKafkaProducerPool(IOptions<KafkaProducerOptions> options)
        {
            _options = options.Value;
            _connectionPoolSize = _options.ConnectionPoolSize;
            _producerPool = new ConcurrentQueue<IProducer<string, byte[]>>();
        }

        public IProducer<string, byte[]> Rent()
        {
            if (_producerPool.TryDequeue(out var producer))
            {
                Interlocked.Decrement(ref _producerReferenceCount);

                return producer;
            }

            var config = KafkaUnderlyingConfigBuilder.Singleton
                .Clear()
                .Append(_options)
                .Build();

            producer = new ProducerBuilder<string, byte[]>(config)
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
