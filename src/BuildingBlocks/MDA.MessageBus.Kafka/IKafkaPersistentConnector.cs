using System;

namespace MDA.MessageBus.Kafka
{
    public interface IKafkaPersistentConnector : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IDisposable CreateChannel();
    }
}
