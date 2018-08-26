using System;

namespace MDA.Messaging
{
    public class SlowMessageHandlerEventArgs : EventArgs
    {
        public SlowMessageHandlerEventArgs(
            string messageName,
            string messageHandlerName,
            TimeSpan elapsed)
        {
            MessageName = messageName;
            MessageHandlerName = messageHandlerName;
            Elapsed = elapsed;
        }

        public string MessageName { get; private set; }
        public string MessageHandlerName { get; private set; }
        public TimeSpan Elapsed { get; private set; }

        public override string ToString()
        {
            return string.Format("SlowMessageHandlerEventArgs [MessageName: {0}, MessageHandlerName: {1}, ElapsedInMilliseconds: {2}]", MessageName, MessageHandlerName, Elapsed.TotalMilliseconds);
        }
    }
}
