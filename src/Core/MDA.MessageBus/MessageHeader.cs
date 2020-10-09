namespace MDA.MessageBus
{
    public class MessageHeader
    {
        public MessageHeader(string key, byte[] value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get;  }

        public byte[] Value { get; }
    }
}
