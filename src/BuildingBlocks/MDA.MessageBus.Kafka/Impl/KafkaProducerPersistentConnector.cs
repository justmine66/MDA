using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MDA.MessageBus.Kafka.Impl
{
    public class KafkaProducerPersistentConnector : AbstractPersistentConnector
    {
        private readonly ILogger<KafkaProducerPersistentConnector> _logger;

        private Producer<Null, string> _connection;
        private bool _disposed;

        public KafkaProducerPersistentConnector(
            ILogger<KafkaProducerPersistentConnector> logger,
            IOptions<KafkaOptions> options)
            : base(logger, options.Value.ProducerSettings)
        {
            _logger = logger;
        }

        public override bool IsConnected => _connection != null && !_disposed;

        public override IDisposable CreateChannel()
        {
            TryConnect();

            return _connection;
        }

        protected override void DoConnect(IEnumerable<KeyValuePair<string, object>> settings)
        {
            _connection = new Producer<Null, string>(settings, null, new StringSerializer(Encoding.UTF8));
            _connection.OnError += OnError;
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _connection.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.ToString());
                }
            }

            _disposed = true;
        }

        private void OnError(object sender, Error error)
        {
            if (_disposed) return;

            _logger.LogWarning($"A Kafka connection throw exception, Trying to re-connect, info:{error}");

            TryConnect();
        }
    }
}
