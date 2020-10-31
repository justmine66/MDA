namespace MDA.MessageBus
{
    /// <summary>
    /// A factory abstraction for a component that can create <see cref="IMessagePublisher"/> instances with custom configuration for a given logical name.
    /// </summary>
    public interface IMessagePublisherFactory
    {
        /// <summary>
        /// Creates and configures an <see cref="IMessagePublisher"/> instance using the configuration that corresponds to the logical name specified by <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The logical name of the message publisher to create.</param>
        /// <returns>A new <see cref="IMessagePublisher"/> instance.</returns>
        IMessagePublisher CreateMessagePublisher(MessageBusClientNames name);
    }
}
