using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Kafka
{
    public interface IKafkaConsumer
    {
        string Group { get; }

        event EventHandler<ConsumeResult<string, byte[]>> OnConsumeReceived;

        Task ConsumeAsync(CancellationToken cancellationToken = default);
    }
}
