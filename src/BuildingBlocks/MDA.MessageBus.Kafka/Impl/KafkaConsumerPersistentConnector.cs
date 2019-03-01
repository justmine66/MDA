using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDA.MessageBus.Kafka.Impl
{
    public class KafkaConsumerPersistentConnector : AbstractPersistentConnector
    {
        private readonly IDeserializer<string> _deserializer;
        private readonly ILogger _logger;

        private Consumer<Null, string> _consumer;
        private bool _disposed;

        public KafkaConsumerPersistentConnector(
            ILogger<KafkaConsumerPersistentConnector> logger,
            IEnumerable<KeyValuePair<string, object>> settings) : base(logger, settings)
        {
            _deserializer = new StringDeserializer(Encoding.UTF8);
            _logger = logger;
        }

        public override bool IsConnected => _consumer != null && !_disposed;
        public override IDisposable CreateChannel()
        {
            TryConnect();
            return _consumer;
        }

        protected override void DoConnect(IEnumerable<KeyValuePair<string, object>> settings)
        {
            _consumer = new Consumer<Null, string>(settings, null, _deserializer);
            _consumer.OnConsumeError += OnConsumeError;
            _consumer.OnError += OnError;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _consumer.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.ToString());
                }
            }

            _disposed = true;
        }

        private void OnConsumeError(object sender, Confluent.Kafka.Message e)
        {
            var message = e.Deserialize<Null, string>(null, _deserializer);
            if (_disposed) return;

            _logger.LogWarning($"An error occurred during consume the message; Topic:'{e.Topic}'," +
                               $"Message:'{message.Value}', Reason:'{e.Error}'.");

            TryConnect();
        }

        private void OnError(object sender, Error error)
        {
            if (_disposed) return;

            _logger.LogWarning($"A Kafka connection throw exception, Trying to re-connect, info:{error}");

            TryConnect();
        }
    }
}
