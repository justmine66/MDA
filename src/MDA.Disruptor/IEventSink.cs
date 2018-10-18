namespace MDA.Disruptor
{
    public interface IEventSink<TEvent>
    {
        /// <summary>
        /// Publishes an event to the ring buffer. It handles claiming the next sequence, getting the current(uninitialised) event from the ring buffer and publishing the claimed sequence after translation.
        /// </summary>
        /// <param name="translator"></param>
        void PublishEvent(IEventTranslator<TEvent> translator);

        /// <summary>
        /// Attempts to publish an event to the ring buffer. It handles claiming the next sequence, getting the current(uninitialised) event from the ring buffer and publishing the claimed sequence after translation.Will return false if specified capacity was not available.
        /// </summary>
        /// <param name="translator">translator the user specified translation for the event</param>
        /// <returns>true if the value was published, false if there was insufficient capacity.</returns>
        bool TryPublishEvent(IEventTranslator<TEvent> translator);

        /// <summary>
        /// Allows one user supplied argument.
        /// </summary>
        /// <typeparam name="TArg">Class of the user supplied argument.</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg">A user supplied argument.</param>
        void PublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg);

        /// <summary>
        /// Allows one user supplied argument.
        /// </summary>
        /// <typeparam name="TArg">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg">A user supplied argument</param>
        /// <returns>true if the value was published, false if there was insufficient capacity.</returns>
        bool TryPublishEvent<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg arg);

        /// <summary>
        /// Allows two user supplied arguments.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        void PublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1);

        /// <summary>
        /// Allows two user supplied arguments.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvent<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0 arg0, TArg1 arg1);

        /// <summary>
        /// Allows three user supplied arguments
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        void PublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2);

        /// <summary>
        /// Allows three user supplied arguments
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvent<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0 arg0, TArg1 arg1, TArg2 arg2);

        /// <summary>
        /// Allows a variable number of user supplied arguments
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        void PublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args);

        /// <summary>
        /// Allows a variable number of user supplied arguments
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvent(IEventTranslatorVarArg<TEvent> translator, params object[] args);

        /// <summary>
        /// Publishes multiple events to the ring buffer. It handles claiming the next sequence, getting the current(uninitialised)
        /// event from the ring buffer and publishing the claimed sequence after translation.
        /// <para/>
        /// With this call the data that is to be inserted into the ring buffer will be a field (either explicitly or captured anonymously),
        /// therefore this call will require an instance of the translator for each value that is to be inserted into the ring buffer.
        /// </summary>
        /// <param name="translators">The user specified translation for each event</param>
        void PublishEvents(IEventTranslator<TEvent>[] translators);

        /// <summary>
        /// Publishes multiple events to the ring buffer.  It handles claiming the next sequence, getting the current(uninitialised)
        /// event from the ring buffer and publishing the claimed sequence after translation.
        /// <para/>
        /// With this call the data that is to be inserted into the ring buffer will be a field (either explicitly or captured anonymously),
        /// therefore this call will require an instance of the translator for each value that is to be inserted into the ring buffer.
        /// </summary>
        /// <param name="translators">The user specified translation for each event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        void PublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize);

        /// <summary>
        /// Attempts to publish multiple events to the ring buffer.  It handles claiming the next sequence, getting the current(uninitialised)
        /// event from the ring buffer and publishing the claimed sequence after translation.Will return false if specified capacity was not available.
        /// </summary>
        /// <param name="translators">The user specified translation for each event</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents(IEventTranslator<TEvent>[] translators);

        /// <summary>
        /// Attempts to publish multiple events to the ring buffer.  It handles claiming the next sequence, getting the current(uninitialised)
        /// event from the ring buffer and publishing the claimed sequence after translation.Will return false if specified capacity was not available.
        /// </summary>
        /// <param name="translators">The user specified translation for each event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents(IEventTranslator<TEvent>[] translators, int batchStartsAt, int batchSize);

        /// <summary>
        /// Allows one user supplied argument per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg">A user supplied argument.</param>
        /// <typeparam name="TArg">Class of the user supplied argument</typeparam>
        void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg);

        /// <summary>
        /// Allows one user supplied argument per event.
        /// </summary>
        /// <typeparam name="TArg">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg">A user supplied argument.</param>
        void PublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, int batchStartsAt, int batchSize, TArg[] arg);

        /// <summary>
        /// Allows one user supplied argument per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg">A user supplied argument.</param>
        /// <typeparam name="TArg">Class of the user supplied argument</typeparam>
        bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, TArg[] arg);

        /// <summary>
        /// Allows one user supplied argument per event.
        /// </summary>
        /// <typeparam name="TArg">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg">A user supplied argument.</param>
        bool TryPublishEvents<TArg>(IEventTranslatorOneArg<TEvent, TArg> translator, int batchStartsAt, int batchSize, TArg[] arg);

        /// <summary>
        /// Allows two user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        void PublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0[] arg0, TArg1[] arg1);

        /// <summary>
        /// Allows two user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        void PublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1);

        /// <summary>
        /// Allows two user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, TArg0[] arg0, TArg1[] arg1);

        /// <summary>
        /// Allows two user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents<TArg0, TArg1>(IEventTranslatorTwoArg<TEvent, TArg0, TArg1> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1);

        /// <summary>
        /// Allows three user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2);

        /// <summary>
        /// Allows three user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        void PublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2);

        /// <summary>
        /// Allows three user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2);

        /// <summary>
        /// Allows three user supplied arguments per event.
        /// </summary>
        /// <typeparam name="TArg0">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg1">Class of the user supplied argument</typeparam>
        /// <typeparam name="TArg2">Class of the user supplied argument</typeparam>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="arg0">A user supplied argument.</param>
        /// <param name="arg1">A user supplied argument.</param>
        /// <param name="arg2">A user supplied argument.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents<TArg0, TArg1, TArg2>(IEventTranslatorThreeArg<TEvent, TArg0, TArg1, TArg2> translator, int batchStartsAt, int batchSize, TArg0[] arg0, TArg1[] arg1, TArg2[] arg2);

        /// <summary>
        /// Allows a variable number of user supplied arguments per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        void PublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args);

        /// <summary>
        /// Allows a variable number of user supplied arguments per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        void PublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args);

        /// <summary>
        /// Allows a variable number of user supplied arguments per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, params object[][] args);

        /// <summary>
        /// Allows a variable number of user supplied arguments per event.
        /// </summary>
        /// <param name="translator">The user specified translation for the event</param>
        /// <param name="batchStartsAt">The first element of the array which is within the batch.</param>
        /// <param name="batchSize">The actual size of the batch.</param>
        /// <param name="args">User supplied arguments, one Object[] per event.</param>
        /// <returns>true if the value was published, false if there was insufficient capacity</returns>
        bool TryPublishEvents(IEventTranslatorVarArg<TEvent> translator, int batchStartsAt, int batchSize, params object[][] args);
    }
}

