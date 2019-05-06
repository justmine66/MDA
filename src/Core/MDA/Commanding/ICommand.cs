using System;
using MDA.Messaging;

namespace MDA.Commanding
{
    public interface ICommand : IMessage
    {
        DateTime ProcessingTime { get; set; }
    }
}
