using System;
using MDA.Disruptor.Impl;
using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Utility
{
    /// <summary>
    /// Factory that creates optimized instance of &lt;see cref="IBatchEventProcessor{T}"/&gt;.
    /// </summary>
    public static class BatchEventProcessorFactory
    {
        /// <summary>
        /// Create a new <see cref="IBatchEventProcessor{T}"/> with dedicated generic arguments.
        /// </summary>
        /// <typeparam name="T">the type of event used.</typeparam>
        /// <param name="dataProvider">dataProvider to which events are published.</param>
        /// <param name="sequenceBarrier">SequenceBarrier on which it is waiting.</param>
        /// <param name="eventHandler">eventHandler is the delegate to which events are dispatched.</param>
        /// <returns></returns>
        public static IBatchEventProcessor<T> Create<T>(
            IDataProvider<T> dataProvider,
            ISequenceBarrier sequenceBarrier,
            IEventHandler<T> eventHandler)
            where T : class
        {
            var dataProviderProxy = StructProxy.CreateProxyInstance(dataProvider);
            var sequenceBarrierProxy = StructProxy.CreateProxyInstance(sequenceBarrier);
            var eventHandlerProxy = StructProxy.CreateProxyInstance(eventHandler);
            var batchStartAwareProxy = eventHandler is IBatchStartAware batchStartAware
                ? StructProxy.CreateProxyInstance(batchStartAware)
                : new NoOpBatchStartAware();

            var batchEventProcessorType = typeof(BatchEventProcessor<>).MakeGenericType(typeof(T), dataProviderProxy.GetType(), sequenceBarrierProxy.GetType(), eventHandlerProxy.GetType(), batchStartAwareProxy.GetType());

            return Activator.CreateInstance(batchEventProcessorType, dataProviderProxy, sequenceBarrierProxy, eventHandlerProxy, batchStartAwareProxy) as IBatchEventProcessor<T>;
        }

        private struct NoOpBatchStartAware : IBatchStartAware
        {
            public void OnBatchStart(long batchSize)
            {
            }
        }
    }
}
