namespace MDA.Messaging
{
    /// <summary>
    /// Contact for handling a <see cref="Message{T}"/>.
    /// </summary>
    public interface IMessageHandler
    {
        void Handle<T>(Message<T> message);
    }
}
