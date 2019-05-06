using System;
using MDA.Messaging;

namespace MDA.Commanding
{
    public abstract class Command : Message, ICommand
    {
        public DateTime ProcessingTime { get; set; }
    }
}
