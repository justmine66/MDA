using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class KafkaMessageProducer : IMessagePublisher, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IKafkaPersistentConnector _connector;

        public KafkaMessageProducer(
            IKafkaPersistentConnectorFactory connectorFactory,
            ILogger<KafkaMessageProducer> logger)
        {
            _connector = connectorFactory.Create(ChannelType.Producer);
            _logger = logger;
        }

        public async Task PublishAsync(string topic, dynamic message)
        {
            await DoPublishAsync(topic, message);
        }

        public async Task PublishAsync<TMessage>(TMessage message)
            where TMessage : Message
        {
            var topic = typeof(TMessage).Name;
            await DoPublishAsync(topic, message);
        }

        public async Task PublishAllAsync(string topic, IEnumerable<dynamic> messages)
        {
            await DoPublishAllAsync(topic, messages);
        }

        public async Task PublishAllAsync<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : Message
        {
            var topic = typeof(TMessage).Name;
            await DoPublishAllAsync(topic, messages);
        }

        public void Dispose()
        {
            _connector?.Dispose();
        }

        private async Task DoPublishAsync(string topic, dynamic message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (!_connector.IsConnected)
            {
                _connector.TryConnect();
            }

            if (!(_connector.CreateChannel() is Producer<Null, string> producer))
                throw new Exception("Unable to establish producer channel.");

            var policy = Policy.Handle<KafkaException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            var body = JsonConvert.SerializeObject(message);
            await policy.Execute(async () => await producer.ProduceAsync(topic, null, body));
        }

        private async Task DoPublishAllAsync(string topic, IEnumerable<dynamic> messages)
        {
            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            if (!_connector.IsConnected)
            {
                _connector.TryConnect();
            }

            if (!(_connector.CreateChannel() is Producer<Null, string> producer))
                throw new Exception("Unable to establish producer channel.");

            var policy = Policy.Handle<KafkaException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            foreach (var message in messages)
            {
                var body = JsonConvert.SerializeObject(message);

                await policy.Execute(async () => await producer.ProduceAsync(topic, null, body));
            }
        }
    }
}
