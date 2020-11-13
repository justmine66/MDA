using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public class DefaultKafkaConsumer : IKafkaConsumer
    {
        private readonly IKafkaConsumerClientFactory _clientFactory;
        private CancellationToken _cancellationToken;

        public DefaultKafkaConsumer(IKafkaConsumerClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task ConsumeAsync(CancellationToken cancellationToken = default)
        {
            _cancellationToken = cancellationToken;

            new Thread(DoConsume)
            {
                Name = "KafkaConsumer.Worker",
                IsBackground = true
            }.Start(this);

            await Task.CompletedTask;
        }

        private void DoConsume(object data)
        {
            using (var client = _clientFactory.CreateClient())
            {
                try
                {
                    while (!_cancellationToken.IsCancellationRequested)
                    {
                        var consumerResult = client.Consume(_cancellationToken);

                        if (consumerResult == null ||
                            consumerResult.IsPartitionEOF ||
                            consumerResult.Message.Value == null) continue;

                        OnConsumeReceived?.Invoke(this, consumerResult);
                    }

                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    client.Close();
                }
            }
        }

        public event EventHandler<ConsumeResult<string, byte[]>> OnConsumeReceived;
    }
}
