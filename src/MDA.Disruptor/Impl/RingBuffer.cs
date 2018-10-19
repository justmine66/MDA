using System;
using MDA.Disruptor.dsl;
using MDA.Disruptor.Extensions;
using MDA.Disruptor.Infrastracture;

namespace MDA.Disruptor.Impl
{
    public abstract class RingBufferFields<TEvent> : LhsPadding
    {
        private static readonly int _bufferPad;
        private static readonly long _refArrayBase;
        private static readonly int _refElementShift;

        static RingBufferFields()
        {
            var scale = IntPtr.Size;

            switch (scale)
            {
                case 4:
                    _refElementShift = 2;
                    break;
                case 8:
                    _refElementShift = 3;
                    break;
                default:
                    throw new PlatformNotSupportedException("Unknown pointer size");
            }

            _bufferPad = 128 / scale;
        }

        private readonly long _indexMask;
        private readonly object[] _entries;
        protected readonly int _bufferSize;
        protected readonly ISequencer _sequencer;

        protected RingBufferFields(
            ISequencer sequencer,
            IEventFactory<TEvent> eventFactory)
        {
            _sequencer = sequencer;
            _bufferSize = _sequencer.BufferSize;

            if (_bufferSize < 1)
            {
                throw new ArgumentException("bufferSize must not be less than 1");
            }

            if (_bufferSize.IsNotPowerOf2())
            {
                throw new ArgumentException("bufferSize must be a power of 2");
            }

            _indexMask = _bufferSize - 1;
            _entries = new object[sequencer.BufferSize + 2 * _bufferPad];

            Fill(eventFactory);
        }

        private void Fill(IEventFactory<TEvent> eventFactory)
        {
            for (int i = 0; i < _bufferSize; i++)
            {
                _entries[_bufferPad + i] = eventFactory.NewInstance();
            }
        }

        protected TEvent ElementAt(long sequence)
        {
            return default(TEvent);
        }
    }

    public sealed class RingBuffer<TEvent> : RingBufferFields<TEvent>, ICursored, IEventSequencer<TEvent>, IEventSink<TEvent>
    {
        /// <summary>
        /// Construct a RingBuffer with the full option set.
        /// </summary>
        /// <param name="sequencer">to handle the ordering of events moving through the RingBuffer.</param>
        /// <param name="eventFactory">to newInstance entries for filling the RingBuffer.</param>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public RingBuffer(
            ISequencer sequencer,
            IEventFactory<TEvent> eventFactory)
            : base(sequencer, eventFactory)
        {

        }

        /// <summary>
        /// Create a new Ring Buffer with the specified producer type (SINGLE or MULTI).
        /// </summary>
        /// <param name="producerType">producer type to use <see cref="ProducerType"/>.</param>
        /// <param name="factory">used to create events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <param name="waitStrategy">used to determine how to wait for new elements to become available.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> Create(
        ProducerType producerType,
        IEventFactory<TEvent> factory,
        int bufferSize,
        IWaitStrategy waitStrategy)
        {
            switch (producerType)
            {
                case ProducerType.Single:
                    return CreateSingleProducer(factory, bufferSize, waitStrategy);
                case ProducerType.Multi:
                    return CreateMultiProducer(factory, bufferSize, waitStrategy);
                default:
                    throw new ArgumentOutOfRangeException(producerType.ToString());
            }
        }

        /// <summary>
        /// Create a new single producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateSingleProducer(IEventFactory<TEvent> factory, int bufferSize)
        {
            return CreateSingleProducer(factory, bufferSize, new BlockingWaitStrategy());
        }

        /// <summary>
        /// Create a new single producer RingBuffer with the specified wait strategy.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="bufferSize"></param>
        /// <param name="waitStrategy"></param>
        /// <returns></returns>
        public static RingBuffer<TEvent> CreateSingleProducer(
            IEventFactory<TEvent> factory,
            int bufferSize,
            IWaitStrategy waitStrategy)
        {
            var sequencer = new SingleProducerSequencer(bufferSize, waitStrategy);

            return new RingBuffer<TEvent>(sequencer, factory);
        }

        /// <summary>
        /// Create a new multiple producer RingBuffer with the specified wait strategy.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <param name="waitStrategy">used to determine how to wait for new elements to become available.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateMultiProducer(
            IEventFactory<TEvent> factory,
            int bufferSize,
            IWaitStrategy waitStrategy)
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
