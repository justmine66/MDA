using System;

namespace MDA.Disruptor.Impl
{
    public class BatchEventProcessor<TEvent, TDataProvider, TSequenceBarrier, TEventHandler, TBatchStartAware>
        : IBatchEventProcessor<TEvent>
        where TEvent : class
        where TDataProvider : IDataProvider<TEvent>
        where TSequenceBarrier : ISequenceBarrier
        where TEventHandler : IEventHandler<TEvent>
        where TBatchStartAware : IBatchStartAware
    {
        public ISequence GetSequence()
        {
            throw new NotImplementedException();
        }

        public void Halt()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public bool IsRunning()
        {
            throw new NotImplementedException();
        }

        public void WaitUntilStarted(TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void SetExceptionHandler(IExceptionHandler<TEvent> exceptionHandler)
        {
            throw new NotImplementedException();
        }
    }
}
