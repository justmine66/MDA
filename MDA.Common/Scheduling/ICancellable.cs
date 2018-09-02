namespace MDA.Common.Scheduling
{
    /// <summary>
    /// Defines the interface used to cancel some event.
    /// </summary>
    public interface ICancellable
    {
        /// <summary>
        /// Cancels the event.
        /// </summary>
        void Cancel();

        /// <summary>
        /// Indicates whether the event has been canceled.
        /// </summary>
        bool Cancelled { get; }
    }
}
