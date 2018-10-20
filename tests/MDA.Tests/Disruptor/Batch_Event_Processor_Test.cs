using System;
using System.Threading;
using MDA.Disruptor;
using MDA.Disruptor.Impl;
using MDA.Tests.Disruptor.Support;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MDA.Tests.Disruptor
{
    public class Batch_Event_Processor_Test
    {
        private readonly RingBuffer<StubEvent> _ringBuffer;
        private readonly ISequenceBarrier _barrier;
        private readonly IExceptionHandler<StubEvent> _exceptionHandler;

        public Batch_Event_Processor_Test()
        {
            _ringBuffer = RingBuffer<StubEvent>.CreateMultiProducer(StubEvent.EventFactory, 16);
            _barrier = _ringBuffer.NewBarrier();

            var provider = new ServiceCollection()
                .AddLogging()
                .AddScoped<IExceptionHandler<StubEvent>, FatalExceptionHandler<StubEvent>>()
                .BuildServiceProvider();

            _exceptionHandler = provider.GetService<IExceptionHandler<StubEvent>>();
        }

        [Fact(DisplayName = "设置空ExceptionHandler，应该抛出参数异常。")]
        public void Should_Throw_Exception_On_Setting_Null_Exception_Handler()
        {
            var processor = new BatchEventProcessor<StubEvent>(_ringBuffer, _barrier, null, _exceptionHandler);

            Assert.Throws<ArgumentNullException>(() => { processor.SetExceptionHandler(null); });
        }

        [Fact(DisplayName = "批处理所有事件都应该调用OnEvent()方法。")]
        public void Should_Call_Methods_In_Lifecycle_Order_For_Batch()
        {
            var eventLatch = new CountdownEvent(3);
            var eventHandler = new LatchEventHandler(eventLatch);
            var processor = new BatchEventProcessor<StubEvent>(_ringBuffer, _barrier, eventHandler, _exceptionHandler);

            _ringBuffer.AddGatingSequences(processor.GetSequence());

            _ringBuffer.Publish(_ringBuffer.Next());
            _ringBuffer.Publish(_ringBuffer.Next());
            _ringBuffer.Publish(_ringBuffer.Next());

            var thread = new Thread(() => processor.Run());
            thread.Start();

            Assert.True(eventLatch.Wait(TimeSpan.FromSeconds(2)));

            processor.Halt();
            thread.Join();
        }

        private class LatchEventHandler : IEventHandler<StubEvent>
        {
            private readonly CountdownEvent _latch;

            public LatchEventHandler(CountdownEvent latch)
            {
                _latch = latch;
            }

            public void OnEvent(StubEvent @event, long sequence, bool endOfBatch)
            {
                _latch.Signal();
            }
        }

        private class LatchExceptionHandler : IExceptionHandler<StubEvent>
        {
            private readonly CountdownEvent _latch;

            public LatchExceptionHandler(CountdownEvent latch)
            {
                _latch = latch;
            }

            public void HandleEventException(Exception ex, long sequence, StubEvent @event)
            {
                _latch.Signal();
            }

            public void HandleOnStartException(Exception ex)
            {

            }

            public void HandleOnShutdownException(Exception ex)
            {

            }
        }

        private class ExceptionEventHandler : IEventHandler<StubEvent>
        {
            public void OnEvent(StubEvent @event, long sequence, bool endOfBatch)
            {
                throw new ArgumentException();
            }
        }
    }
}
