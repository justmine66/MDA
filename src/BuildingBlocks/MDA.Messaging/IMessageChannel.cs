namespace MDA.Messaging
{
    /// <summary>
    /// Channels to deliver messages. 
    /// Sends <see cref="Message{T}"/> to this <see cref="IMessageChannel"/>.
    /// </summary>
    public interface IMessageChannel
    {
        void Send<T>(Message<T> message);
    }
}
