using System;

namespace MDA.Domain
{
    [Flags]
    public enum DomainMessageTypes
    {
        Command = 1 << 0,
        Event = 1 << 1,
        Notification = 1 << 2,
    }
}
