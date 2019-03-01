using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;

namespace MDA.MessageBus.Kafka.Impl
{
    public abstract class AbstractPersistentConnector : IKafkaPersistentConnector
    {
        private readonly ILogger _logger;
        private readonly object _latch = new object();
        private readonly IEnumerable<KeyValuePair<string, object>> _settings;

        protected AbstractPersistentConnector(
            ILogger<AbstractPersistentConnector> logger,
            IEnumerable<KeyValuePair<string, object>> settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract bool IsConnected { get; }

        public bool TryConnect()
        {
            _logger.LogInformation("Kafka Producer is trying to connect...");

            lock (_latch)
            {
                var policy = Policy.Handle<KafkaException>()
                    .WaitAndRetry(
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        (ex, time) =>
                        {
                            _logger.LogWarning(ex.ToString());

                        });

                policy.Execute(() => DoConnect(_settings));

                if (IsConnected)
                {
                    return true;
                }

                _logger.LogCritical("FATAL ERROR: Kafka connections could not be created and opened");

                return false;
            }
        }

        public abstract IDisposable CreateChannel();

        protected abstract void DoConnect(IEnumerable<KeyValuePair<string, object>> settings);
        protected abstract void Dispose(bool disposing);

        ~AbstractPersistentConnector()
        {
            Dispose(false);
        }
    }
}
