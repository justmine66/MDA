using Confluent.Kafka;
using MDA.Shared.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class KafkaAsyncMessageQueueService : IAsyncMessageQueueService
    {
        private readonly IProducerConnectionPool _connectionPool;
        private readonly IBinarySerializer _serializer;
        private readonly ILogger _logger;

        public KafkaAsyncMessageQueueService(
            IProducerConnectionPool connectionPool,
            ILogger logger,
            IBinarySerializer serializer)
        {
            _connectionPool = connectionPool;
            _logger = logger;
            _serializer = serializer;
        }

        public async Task StartAsync(CancellationToken token = default) 
            => await Task.CompletedTask;

        public async Task EnqueueAsync(IMessage message, CancellationToken token = default)
        {
            var topic = message.Topic;
            var producer = _connectionPool.RentProducer();

            try
            {
                var result = await producer.ProduceAsync(topic, new Message<string, byte[]>
                {
                    Key = message.Id,
                    Value = _serializer.Serialize(_serializer)
                }, token);

                if (result.Status == PersistenceStatus.Persisted ||
                    result.Status == PersistenceStatus.PossiblyPersisted)
                {
                    if (_logger.IsEnabled(LogLevel.Debug))
                        _logger.LogDebug($"The kafka message was published successfully, Topic: [{topic}], Message: [{message.Id}, {message.GetType().FullName}].");

                    return;
                }

                _logger.LogError("The kafka message publish failed!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Publishing the kafka message, Topic: [{topic}], Message: [{message.Id}, {message.GetType().FullName}], has a unknown exception: {ex}.");
            }
            finally
            {
                var returned = _connectionPool.Return(producer);

                if (!returned)
                {
                    producer.Dispose();
                }
            }
        }

        public async Task StopAsync(CancellationToken token = default) 
            => await Task.CompletedTask;
    }
}
