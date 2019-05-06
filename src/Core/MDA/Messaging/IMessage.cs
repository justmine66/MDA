using System;

namespace MDA.Messaging
{
    public interface IMessage
    {
        string Id { get; set; }
        DateTime Timestamp { get; set; }
    }
}
