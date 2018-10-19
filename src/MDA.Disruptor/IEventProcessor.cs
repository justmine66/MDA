using MDA.Disruptor.Impl;

namespace MDA.Disruptor
{
    /// <summary>
    /// An EventProcessor needs to be an implementation of a runnable that will poll for events from the <see cref="RingBuffer{TEvent}"/> using the appropriate wait strategy. It is unlikely that you will need to implement this interface yourself. Look at using the <see cref="IEventHandler{TEvent}"/> interface along with the pre-supplied BatchEventProcessor in the first instance.
    /// </summary>
    public interface IEventProcessor : IRunnable
    {
        /// <summary>
        /// Return a reference to the <see cref="ISequence"/> being used by this <see cref="IEventProcessor"/>
        /// </summary>
        ISequence GetSequence();

        /// <summary>
        /// Signal that this <see cref="IEventProcessor"/> should stop when it has finished consuming at the next clean break.
        /// It will call <see cref="ISequenceBarrier.Alert"/> to notify the thread to check status.
        /// </summary>
        void Halt();

        /// <summary>
        /// Gets if the processor is running.
        /// </summary>
        /// <returns></returns>
        bool IsRunning();
    }
}
