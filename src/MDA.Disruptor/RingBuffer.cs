using System;

namespace MDA.Disruptor
{
    public sealed class RingBuffer<TEvent> : RingBufferFields<TEvent>, ICursored, IEventSequencer<TEvent>, IEventSink<TEvent>
    {
        public RingBuffer(
            ISequencer sequencer,
            IEventFactory<TEvent> eventFactory)
            : base(sequencer, eventFactory)
        {

        }

        /// <summary>
        /// Create a new single producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateSingleProducer(Func<TEvent> factory, int bufferSize)
        {
            return null;
        }

        public int BufferSize => throw new NotImplementedException();

        public TEvent Get(long sequence)
        {
            throw new NotImplementedException();
        }

        public long GetCursor()
        {
            throw new NotImplementedException();
        }

        public long GetRemainingCapacity()
        {
            throw new NotImplementedException();
        }

        public bool HasAvailableCapacity(int requiredCapacity)
        {
            throw new NotImplementedException();
        }

        public long Next()
        {
            throw new NotImplementedException();
        }

        public long Next(int n)
        {
            throw new NotImplementedException();
        }

        public void Publish(long sequence)
        {
            throw new NotImplementedException();
        }

        public void Publish(long lo, long hi)
        {
            throw new NotImplementedException();
        }

        public void PublishEvent(IEventTranslator<TEvent> translator)
        {
            throw new NotImplementedException();
        }

        public void PublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg0)
        {
            throw new NotImplementedException();
        }

        public void PublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1)
        {
            throw new NotImplementedException();
        }

        public void PublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            throw new NotImplementedException();
        }

        public void PublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents(IEventTranslator<TEvent>[] translators)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg0)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, int batchStartsAt, int batchSize, TArg[] arg0)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, A[] arg0, B[] arg1)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, int batchStartsAt, int batchSize, A[] arg0, B[] arg1)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args)
        {
            throw new NotImplementedException();
        }

        public void PublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args)
        {
            throw new NotImplementedException();
        }

        public bool TryNext(out long sequence)
        {
            throw new NotImplementedException();
        }

        public bool TryNext(int n, out long sequence)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvent(IEventTranslator<TEvent> translator)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg0)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents(IEventTranslator<TEvent>[] translators)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg0)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, int batchStartsAt, int batchSize, TArg[] arg0)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, A[] arg0, B[] arg1)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, int batchStartsAt, int batchSize, A[] arg0, B[] arg1)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args)
        {
            throw new NotImplementedException();
        }

        public bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args)
        {
            throw new NotImplementedException();
        }
    }
}
