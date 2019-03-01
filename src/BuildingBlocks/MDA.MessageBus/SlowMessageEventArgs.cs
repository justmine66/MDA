using System;

namespace MDA.MessageBus
{
    public class SlowMessageEventArgs
    {
        public SlowMessageEventArgs(
            string topic,
            string messageName,
            string messageHandlerName,
            TimeSpan elapsed,
            string message)
        {
            Topic = topic;
            MessageName = messageName;
            MessageHandlerName = messageHandlerName;
            Elapsed = elapsed;
            Message = message;
        }

        public string Topic { get; }
        public string MessageName { get; }
        public string MessageHandlerName { get; }
        public TimeSpan Elapsed { get; }

        public string Message { get; }

        public override string ToString()
        {
            return
                $"{{Topic: {Topic}, MessageName: {MessageName}, MessageHandlerName: {MessageHandlerName}, ElapsedInMilliseconds: {Elapsed.TotalMilliseconds}, Message:{Message}}}";
        }
    }
}
