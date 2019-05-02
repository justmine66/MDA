namespace MDA.Messaging
{
    /// <summary>
    /// Channels supporting pull models.
    /// </summary>
    public interface IPollableChannel
    {
        void Receive();
    }
}
