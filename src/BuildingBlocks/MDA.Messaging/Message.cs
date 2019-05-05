namespace MDA.Messaging
{
    public class Message<T>
    {
        public MessageHeader Header { get; set; }
        public T Payload { get; set; }
    }
}
