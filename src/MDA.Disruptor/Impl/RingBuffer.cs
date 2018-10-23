using MDA.Disruptor.dsl;
using MDA.Disruptor.Exceptions;
using MDA.Disruptor.Utility;
using System;

namespace MDA.Disruptor.Impl
{
    /// <summary>
    /// Ring based store of reusable entries containing the data representing an event being exchanged between event producer and <see cref="IEventProcessor"/>s.
    /// </summary>
    /// <typeparam name="TEvent">implementation storing the data for sharing during exchange or parallel coordination of an event.</typeparam>
    public sealed class RingBuffer<TEvent> : RingBufferFields<TEvent>, ICursored, IEventSequencer<TEvent>, IEventSink<TEvent>
    {
        protected long P9, P10, P11, P12, P13, P14, P15;

        #region [ Constructor and factory methods ]

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
        /// Construct a RingBuffer with the full option set.
        /// </summary>
        /// <param name="sequencer">to handle the ordering of events moving through the RingBuffer.</param>
        /// <param name="eventFactory">to newInstance entries for filling the RingBuffer.</param>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public RingBuffer(
            ISequencer sequencer,
            Func<TEvent> eventFactory)
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
                    throw new IllegalStateException(producerType.ToString());
            }
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
            Func<TEvent> factory,
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
                    throw new IllegalStateException(producerType.ToString());
            }
        }

        /// <summary>
        /// Create a new single producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateSingleProducer(
            IEventFactory<TEvent> factory,
            int bufferSize)
        {
            return CreateSingleProducer(factory, bufferSize, new BlockingWaitStrategy());
        }

        /// <summary>
        /// Create a new single producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateSingleProducer(
            Func<TEvent> factory,
            int bufferSize)
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
        /// Create a new single producer RingBuffer with the specified wait strategy.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="bufferSize"></param>
        /// <param name="waitStrategy"></param>
        /// <returns></returns>
        public static RingBuffer<TEvent> CreateSingleProducer(
            Func<TEvent> factory,
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
            var sequencer = new MultiProducerSequencer(bufferSize, waitStrategy);

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
            Func<TEvent> factory,
            int bufferSize,
            IWaitStrategy waitStrategy)
        {
            var sequencer = new MultiProducerSequencer(bufferSize, waitStrategy);

            return new RingBuffer<TEvent>(sequencer, factory);
        }

        /// <summary>
        /// Create a new multiple producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateMultiProducer(
            IEventFactory<TEvent> factory,
            int bufferSize)
        {
            return CreateMultiProducer(factory, bufferSize, new BlockingWaitStrategy());
        }

        /// <summary>
        /// Create a new multiple producer RingBuffer using the default wait strategy <see cref="BlockingWaitStrategy"/>.
        /// </summary>
        /// <param name="factory">used to create the events within the ring buffer.</param>
        /// <param name="bufferSize">number of elements to create within the ring buffer.</param>
        /// <returns>a constructed ring buffer.</returns>
        /// <exception cref="ArgumentException">if bufferSize is less than 1 or not a power of 2</exception>
        public static RingBuffer<TEvent> CreateMultiProducer(
            Func<TEvent> factory,
            int bufferSize)
        {
            return CreateMultiProducer(factory, bufferSize, new BlockingWaitStrategy());
        }

        #endregion

        /// <summary>
        /// Get the event for a given sequence in the RingBuffer.
        /// This call has 2 uses.
        /// 1. Firstly use this call when publishing to a ring buffer.
        /// 2. After calling <see cref="RingBuffer{TEvent}.Next()"/> use this call to get hold of the pre-allocated event to fill with data before calling <see cref="RingBuffer{TEvent}.Publish(long)"/>.
        /// </summary>
        /// <remarks>
        /// Secondly use this call when consuming data from the ring buffer. After calling <see cref="ISequenceBarrier.WaitFor(long)"/> call this method with any value greater than that your current consumer sequence and less than or equal to the value returned from the <see cref="ISequenceBarrier.WaitFor(long)"/> method.
        /// </remarks>
        /// <param name="sequence">for the event</param>
        /// <returns>the event for the given sequence</returns>
        public TEvent Get(long sequence)
        {
            return ElementAt(sequence);
        }

        /// <summary>
        /// Get the current cursor value for the ring buffer. The actual value received will depend on the type of <see cref="ISequencer"/> that is being used.
        /// </summary>
        /// <returns></returns>
        public long GetCursor()
        {
            return Sequencer.GetCursor();
        }

        public long GetRemainingCapacity()
        {
            return Sequencer.GetRemainingCapacity();
        }

        /// <summary>
        /// The size of the buffer.
        /// </summary>
        /// <returns></returns>
        public int GetBufferSize()
        {
            return BufferSize;
        }

        /// <summary>
        /// Given specified <param name="requiredCapacity"></param> determines if that amount of space is available.Note, you can not assume that if this method returns <c>true</c> that a call to <see cref="RingBuffer{TEvent}.Next()"/> will not block. Especially true if this ring buffer is set up to handle multiple producers.
        /// </summary>
        /// <param name="requiredCapacity"></param>
        /// <returns></returns>
        public bool HasAvailableCapacity(int requiredCapacity)
        {
            return Sequencer.HasAvailableCapacity(requiredCapacity);
        }

        /// <summary>
        /// Increment and return the next sequence for the ring buffer. Calls of this method should ensure that they always publish the sequence afterward.
        /// </summary>
        /// <code>
        /// long sequence = ringBuffer.Next();
        /// try {
        ///     Event e = ringBuffer.Get(sequence);
        ///     // Do some work with the event.
        /// } finally {
        ///     ringBuffer.Publish(sequence);
        /// }
        /// </code>
        /// <returns>The next sequence to publish to.</returns>
        public long Next()
        {
            return Sequencer.Next();
        }

        /// <summary>
        /// The same functionality as <see cref="RingBuffer{TEvent}.Next()"/>, but allows the caller to claim the next n sequences.
        /// </summary>
        /// <param name="n">number of slots to claim</param>
        /// <returns>sequence number of the highest slot claimed.</returns>
        public long Next(int n)
        {
            return Sequencer.Next(n);
        }

        /// <summary>
        /// Increment and return the next sequence for the ring buffer. Calls of this method should ensure that they always publish the sequence afterward.
        /// <code>
        /// long sequence = ringBuffer.Next();
        /// try {
        ///     Event e = ringBuffer.Get(sequence);
        ///     // Do some work with the event.
        /// } finally {
        ///     ringBuffer.Publish(sequence);
        /// }
        /// </code>
        /// </summary>
        /// <param name="sequence">The next sequence to publish to.</param>
        /// <returns></returns>
        /// <exception cref="InsufficientCapacityException">
        /// This method will not block if there is not space available in the ring buffer, instead it will throw an <see cref="InsufficientCapacityException"/>.</exception>
        public bool TryNext(out long sequence)
        {
            return Sequencer.TryNext(out sequence);
        }

        /// <summary>
        /// The same functionality as <see cref="RingBuffer{TEvent}.TryNext(out long)"/>, but allows the caller to attempt to claim the next n sequences.
        /// </summary>
        /// <param name="n">number of slots to claim</param>
        /// <param name="sequence">number of the highest slot claimed</param>
        /// <returns></returns>
        /// <exception cref="InsufficientCapacityException">
        /// This method will not block if there is not space available in the ring buffer, instead it will throw an <see cref="InsufficientCapacityException"/>.</exception>
        public bool TryNext(int n, out long sequence)
        {
            return Sequencer.TryNext(n, out sequence);
        }

        /// <summary>
        /// Sets the cursor to a specific sequence and returns the pre-allocated entry that is stored there. This can cause a data race and should only be done in controlled circumstances, e.g.during initialisation.
        /// </summary>
        /// <param name="sequence">The sequence to claim.</param>
        /// <returns>The pre-allocated event.</returns>
        public TEvent ClaimAndGetPreAllocated(long sequence)
        {
            Sequencer.Claim(sequence);
            return Get(sequence);
        }

        /// <summary>
        /// Add the specified gating sequences to this instance of the Disruptor. They will safely and atomically added to the list of gating sequences.
        /// </summary>
        /// <param name="gatingSequences">The sequences to add.</param>
        public void AddGatingSequences(params ISequence[] gatingSequences)
        {
            Sequencer.AddGatingSequences(gatingSequences);
        }

        /// <summary>
        /// Get the minimum sequence value from all of the gating sequences added to this ringBuffer.
        /// </summary>
        /// <returns></returns>
        public long GetMinimumGatingSequence()
        {
            return Sequencer.GetMinimumSequence();
        }

        /// <summary>
        /// Remove the specified sequence from this ringBuffer.
        /// </summary>
        /// <param name="sequence">sequence to be removed.</param>
        /// <returns>true if this sequence was found, false otherwise.</returns>
        public bool RemoveGatingSequence(ISequence sequence)
        {
            return Sequencer.RemoveGatingSequence(sequence);
        }

        /// <summary>
        /// Create a new <see cref="ISequenceBarrier"/> to be used by an <see cref="IEventProcessor"/> to track which messages are available to be read from the ring buffer given a list of sequences to track.
        /// </summary>
        /// <param name="sequencesToTrack"></param>
        /// <returns></returns>
        public ISequenceBarrier NewBarrier(params ISequence[] sequencesToTrack)
        {
            return Sequencer.NewBarrier(sequencesToTrack);
        }

        /// <summary>
        /// Creates an event poller for this ring buffer gated on the supplied sequences.
        /// </summary>
        /// <param name="gatingSequences">to be gated on.</param>
        /// <returns>A poller that will gate on this ring buffer and the supplied sequences.</returns>
        public EventPoller<TEvent> NewPoller(params ISequence[] gatingSequences)
        {
            return Sequencer.NewPoller(this, gatingSequences);
        }

        public void Publish(long sequence)
        {
            Sequencer.Publish(sequence);
        }

        public void Publish(long lo, long hi)
        {
            Sequencer.Publish(lo, hi);
        }

        /// <summary>
        /// <see cref="IEventSink{TEvent}.PublishEvent(IEventTranslator{TEvent})"/>
        /// </summary>
        /// <param name="translator"></param>
        public void PublishEvent(IEventTranslator<TEvent> translator)
        {
            var sequence = Sequencer.Next();
            TranslateAndPublish(translator, sequence);
        }

        public void PublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg0)
        {
            var sequence = Sequencer.Next();
            TranslateAndPublish(translator, sequence, arg0);
        }

        public void PublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1)
        {
            var sequence = Sequencer.Next();
            TranslateAndPublish(translator, sequence, arg0, arg1);
        }

        public void PublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            var sequence = Sequencer.Next();
            TranslateAndPublish(translator, sequence, arg0, arg1, arg2);
        }

        public void PublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args)
        {
            var sequence = Sequencer.Next();
            TranslateAndPublish(translator, sequence, args);
        }

        public void PublishEvents(IEventTranslator<TEvent>[] translators)
        {
            PublishEvents(translators, 0, translators.Length);
        }

        public void PublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize)
        {
            CheckBounds(translators, batchStartsAt, batchSize);
            var finalSequence = Sequencer.Next(batchSize);
            TranslateAndPublishBatch(translators, batchStartsAt, batchSize, finalSequence);
        }

        public void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg0)
        {
            PublishEvents(translator, 0, arg0.Length, arg0);
        }

        public void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translators, int batchStartsAt, int batchSize, TArg[] arg0)
        {
            CheckBounds(arg0, batchStartsAt, batchSize);
            var finalSequence = Sequencer.Next(batchSize);
            TranslateAndPublishBatch(translators, arg0, batchStartsAt, batchSize, finalSequence);
        }

        public void PublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translators, TArg0[] arg0, TArg1[] arg1)
        {
            PublishEvents(translators, 0, arg0.Length, arg0, arg1);
        }

        public void PublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translators, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1)
        {
            CheckBounds(arg0, arg1, batchStartsAt, batchSize);
            var finalSequence = Sequencer.Next(batchSize);
            TranslateAndPublishBatch(translators, arg0, arg1, batchStartsAt, batchSize, finalSequence);
        }

        public void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            PublishEvents(translator, 0, arg0.Length, arg0, arg1, arg2);
        }

        public void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            CheckBounds(arg0, arg1, arg2, batchStartsAt, batchSize);
            var finalSequence = Sequencer.Next(batchSize);
            TranslateAndPublishBatch(translator, arg0, arg1, arg2, batchStartsAt, batchSize, finalSequence);
        }

        public void PublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args)
        {
            PublishEvents(translator, 0, args.Length, args);
        }

        public void PublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args)
        {
            CheckBounds(batchStartsAt, batchSize, args);
            var finalSequence = Sequencer.Next(batchSize);
            TranslateAndPublishBatch(translator, batchStartsAt, batchSize, finalSequence, args);
        }

        /// <summary>
        /// <see cref="IEventSink{TEvent}.TryPublishEvent(IEventTranslator{TEvent})"/>
        /// </summary>
        /// <param name="translator"></param>
        /// <returns></returns>
        public bool TryPublishEvent(IEventTranslator<TEvent> translator)
        {
            try
            {
                if (Sequencer.TryNext(out long sequence))
                {
                    TranslateAndPublish(translator, sequence);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg0)
        {
            try
            {
                Sequencer.TryNext(out long sequence);
                TranslateAndPublish(translator, sequence, arg0);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1)
        {
            try
            {
                Sequencer.TryNext(out long sequence);
                TranslateAndPublish(translator, sequence, arg0, arg1);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            try
            {
                Sequencer.TryNext(out long sequence);
                TranslateAndPublish(translator, sequence, arg0, arg1, arg2);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args)
        {
            try
            {
                Sequencer.TryNext(out long sequence);
                TranslateAndPublish(translator, sequence, args);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvents(IEventTranslator<TEvent>[] translators)
        {
            return TryPublishEvents(translators, 0, translators.Length);
        }

        public bool TryPublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize)
        {
            CheckBounds(translators, batchStartsAt, batchSize);
            try
            {
                Sequencer.TryNext(batchSize, out long finalSequence);
                TranslateAndPublishBatch(translators, batchStartsAt, batchSize, finalSequence);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg0)
        {
            return TryPublishEvents(translator, 0, arg0.Length, arg0);
        }

        public bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, int batchStartsAt, int batchSize, TArg[] arg0)
        {
            CheckBounds(arg0, batchStartsAt, batchSize);
            try
            {
                Sequencer.TryNext(batchSize, out long finalSequence);
                TranslateAndPublishBatch(translator, arg0, batchStartsAt, batchSize, finalSequence);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, A[] arg0, B[] arg1)
        {
            return TryPublishEvents(translator, 0, arg0.Length, arg0, arg1);
        }

        public bool TryPublishEvents<A, B>(IEventTranslatorTwoArg<TEvent, A, B> translator, int batchStartsAt, int batchSize, A[] arg0, B[] arg1)
        {
            CheckBounds(arg0, arg1, batchStartsAt, batchSize);
            try
            {
                Sequencer.TryNext(batchSize, out long finalSequence);
                TranslateAndPublishBatch(translator, arg0, arg1, batchStartsAt, batchSize, finalSequence);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            return TryPublishEvents(translator, 0, arg0.Length, arg0, arg1, arg2);
        }

        public bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2)
        {
            CheckBounds(arg0, arg1, arg2, batchStartsAt, batchSize);
            try
            {
                if (Sequencer.TryNext(batchSize, out long finalSequence))
                {
                    TranslateAndPublishBatch(translator, arg0, arg1, arg2, batchStartsAt, batchSize, finalSequence);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        public bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args)
        {
            return TryPublishEvents(translator, 0, args.Length, args);
        }

        public bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args)
        {
            CheckBounds(args, batchStartsAt, batchSize);
            try
            {
                Sequencer.TryNext(batchSize, out long finalSequence);
                TranslateAndPublishBatch(translator, batchStartsAt, batchSize, finalSequence, args);
                return true;
            }
            catch (InsufficientCapacityException e)
            {
                return false;
            }
        }

        #region [ private methods ]

        private void TranslateAndPublish(IEventTranslator<TEvent> translator, long sequence)
        {
            try
            {
                translator.TranslateTo(Get(sequence), sequence);
            }
            finally
            {
                Sequencer.Publish(sequence);
            }
        }

        private void TranslateAndPublish<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, long sequence, TArg arg)
        {
            try
            {
                translator.TranslateTo(Get(sequence), sequence, arg);
            }
            finally
            {
                Sequencer.Publish(sequence);
            }
        }

        private void TranslateAndPublish<TArg0, TArg1>(
            IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator,
            long sequence, TArg0 arg0,
            TArg1 arg1)
        {
            try
            {
                translator.TranslateTo(Get(sequence), sequence, arg0, arg1);
            }
            finally
            {
                Sequencer.Publish(sequence);
            }
        }

        private void TranslateAndPublish<TArg0, TArg1, TArg2>(
            IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator,
            long sequence,
            TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            try
            {
                translator.TranslateTo(Get(sequence), sequence, arg0, arg1, arg2);
            }
            finally
            {
                Sequencer.Publish(sequence);
            }
        }

        private void TranslateAndPublish(IEventTranslatorVarArg<TEvent> translator, long sequence, params object[] args)
        {
            try
            {
                translator.TranslateTo(Get(sequence), sequence, args);
            }
            finally
            {
                Sequencer.Publish(sequence);
            }
        }

        private void TranslateAndPublishBatch(
            IEventTranslator<TEvent>[] translators,
            int batchStartsAt,
            int batchSize,
            long finalSequence)
        {
            var initialSequence = finalSequence - (batchSize - 1);
            try
            {
                var sequence = initialSequence;
                var batchEndsAt = batchStartsAt + batchSize;
                for (int i = batchStartsAt; i < batchEndsAt; i++)
                {
                    var translator = translators[i];
                    translator.TranslateTo(Get(sequence), sequence++);
                }
            }
            finally
            {
                Sequencer.Publish(initialSequence, finalSequence);
            }
        }

        private void TranslateAndPublishBatch<TArg>(
            IEventTranslatorOneArg<TEvent, TArg> translator,
            TArg[] arg0,
            int batchStartsAt,
            int batchSize,
            long finalSequence)
        {
            var initialSequence = finalSequence - (batchSize - 1);
            try
            {
                var sequence = initialSequence;
                var batchEndsAt = batchStartsAt + batchSize;
                for (int i = batchStartsAt; i < batchEndsAt; i++)
                {
                    translator.TranslateTo(Get(sequence), sequence++, arg0[i]);
                }
            }
            finally
            {
                Sequencer.Publish(initialSequence, finalSequence);
            }
        }

        private void TranslateAndPublishBatch<TArg0, TArg1>(
            IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator,
            TArg0[] arg0, TArg1[] arg1,
            int batchStartsAt, int batchSize,
            long finalSequence)
        {
            var initialSequence = finalSequence - (batchSize - 1);
            try
            {
                var sequence = initialSequence;
                var batchEndsAt = batchStartsAt + batchSize;
                for (int i = batchStartsAt; i < batchEndsAt; i++)
                {
                    translator.TranslateTo(Get(sequence), sequence++, arg0[i], arg1[i]);
                }
            }
            finally
            {
                Sequencer.Publish(initialSequence, finalSequence);
            }
        }

        private void TranslateAndPublishBatch<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator,
            TArg0[] arg0, TArg1[] arg1, TArg2[] arg2,
            int batchStartsAt, int batchSize,
            long finalSequence)
        {
            var initialSequence = finalSequence - (batchSize - 1);
            try
            {
                var sequence = initialSequence;
                var batchEndsAt = batchStartsAt + batchSize;
                for (int i = batchStartsAt; i < batchEndsAt; i++)
                {
                    translator.TranslateTo(Get(sequence), sequence++, arg0[i], arg1[i], arg2[i]);
                }
            }
            finally
            {
                Sequencer.Publish(initialSequence, finalSequence);
            }
        }

        private void TranslateAndPublishBatch(
            IEventTranslatorVarArg<TEvent> translator, int batchStartsAt,
             int batchSize, long finalSequence, params object[][] args)
        {
            var initialSequence = finalSequence - (batchSize - 1);
            try
            {
                long sequence = initialSequence;
                var batchEndsAt = batchStartsAt + batchSize;
                for (int i = batchStartsAt; i < batchEndsAt; i++)
                {
                    translator.TranslateTo(Get(sequence), sequence++, args[i]);
                }
            }
            finally
            {
                Sequencer.Publish(initialSequence, finalSequence);
            }
        }

        private void CheckBounds<TArg>(TArg[] arg0, int batchStartsAt, int batchSize)
        {
            CheckBatchSizing(batchStartsAt, batchSize);
            BatchOverRuns(arg0, batchStartsAt, batchSize);
        }

        private void CheckBounds<TArg0, TArg1>(TArg0[] arg0, TArg1[] arg1, int batchStartsAt, int batchSize)
        {
            CheckBatchSizing(batchStartsAt, batchSize);
            BatchOverRuns(arg0, batchStartsAt, batchSize);
            BatchOverRuns(arg1, batchStartsAt, batchSize);
        }

        private void CheckBounds<TArg0, TArg1, TArg2>(
        TArg0[] arg0, TArg1[] arg1, TArg2[] arg2, int batchStartsAt, int batchSize)
        {
            CheckBatchSizing(batchStartsAt, batchSize);
            BatchOverRuns(arg0, batchStartsAt, batchSize);
            BatchOverRuns(arg1, batchStartsAt, batchSize);
            BatchOverRuns(arg2, batchStartsAt, batchSize);
        }

        private void CheckBounds(int batchStartsAt, int batchSize, object[][] args)
        {
            CheckBatchSizing(batchStartsAt, batchSize);
            BatchOverRuns(args, batchStartsAt, batchSize);
        }

        private void CheckBatchSizing(int batchStartsAt, int batchSize)
        {
            if (batchStartsAt < 0 || batchSize < 0)
            {
                throw new ArgumentException("Both batchStartsAt and batchSize must be positive but got: batchStartsAt " + batchStartsAt + " and batchSize " + batchSize);
            }
            else if (batchSize > BufferSize)
            {
                throw new ArgumentException("The ring buffer cannot accommodate " + batchSize + " it only has space for " + BufferSize + " entities.");
            }
        }

        private void BatchOverRuns<TArg>(TArg[] arg0, int batchStartsAt, int batchSize)
        {
            if (batchStartsAt + batchSize > arg0.Length)
            {
                throw new ArgumentException(
                    "A batchSize of: " + batchSize +
                        " with batchStatsAt of: " + batchStartsAt +
                        " will overrun the available number of arguments: " + (arg0.Length - batchStartsAt));
            }
        }

        #endregion
    }
}
