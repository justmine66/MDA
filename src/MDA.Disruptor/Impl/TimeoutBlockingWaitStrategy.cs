using System;
using MDA.Disruptor.Exceptions;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Blocking strategy that uses a lock and condition variable for <see cref="IEventProcessor"/>s waiting on a barrier. However it will periodically wake up if it has been idle for specified period by throwing a <see cref="TimeoutException"/>. To make use of this, the event handler class should implement the <see cref="ITimeoutHandler"/>, which the <see cref="IBatchEventProcessor{TEvent}"/> will call if the timeout occurs. <p> This strategy can be used when throughput and low-latency are not as important as CPU resource.
    /// </summary>
    public class TimeoutBlockingWaitStrategy : IWaitStrategy
    {
        public void SignalAllWhenBlocking()
        {
            throw new NotImplementedException();
        }

        public long WaitFor(long sequence, ISequence cursor, ISequence dependentSequence, ISequenceBarrier barrier)
        {
            throw new NotImplementedException();
        }
    }
}
