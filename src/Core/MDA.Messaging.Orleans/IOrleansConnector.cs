using Orleans;
using System;

namespace MDA.Messaging.Orleans
{
    public interface IOrleansConnector : IDisposable
    {
        bool IsConnected { get; }
        IClusterClient CreateConnect();
    }
}
