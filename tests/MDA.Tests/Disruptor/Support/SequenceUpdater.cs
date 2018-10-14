using MDA.Disruptor;
using MDA.Disruptor.Impl;
using System;
using System.Threading;

namespace MDA.Tests.Disruptor.Support
{
    public class SequenceUpdater
    {
        public ISequence sequence = new Sequence();
        private CountdownEvent _barrier = new CountdownEvent(2);
        private long _sleepTime;
        private IWaitStrategy _waitStrategy;

        public SequenceUpdater(long sleepTime, IWaitStrategy waitStrategy)
        {
            _sleepTime = sleepTime;
            _waitStrategy = waitStrategy;
        }

        public void Run()
        {
            try
            {
                _barrier.Signal();
                _barrier.Wait();
                if (0 != _sleepTime)
                {
                    Thread.Sleep((int)_sleepTime);
                }

                sequence.IncrementAndGet();
                _waitStrategy.SignalAllWhenBlocking();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
