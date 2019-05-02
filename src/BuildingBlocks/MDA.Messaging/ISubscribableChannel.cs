namespace MDA.Messaging
{
    /// <summary>
    /// Channels that support push models (publish/subscribe models).
    /// </summary>
    public interface ISubscribableChannel
    {
        void Subscribe(IMessageHandler handler);
        void UnSubscribe(IMessageHandler handler);
    }
}
