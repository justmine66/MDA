using System;

namespace MDA.Domain
{
    [Flags]
    public enum DomainMessageType
    {
        Command = 1 << 0,
        Event = 1 << 1,
        Notification = 1 << 2,
    }
}
