using System;

namespace MDA.MessageBus
{
    [Flags]
    public enum MessageBusClientNames
    {
        Disruptor = 1 << 0,
        Kafka = 1 << 1,
        RabbitMQ = 1 << 2
    }
}
