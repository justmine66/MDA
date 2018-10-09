using System;

namespace MDA.Disruptor.Impl
{
    class BatchEventProcessor<T, TDataProvider, TSequenceBarrier, TEventHandler, TBatchStartAware>
        : IBatchEventProcessor<T>
        where T : class
        where TDataProvider : IDataProvider<T>
        where TSequenceBarrier : ISequenceBarrier
        where TEventHandler : IEventHandler<T>
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

        public void SetExceptionHandler(IExceptionHandler<T> exceptionHandler)
        {
            throw new NotImplementedException();
        }
    }
}
