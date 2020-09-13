using Disruptor.Dsl;
using System;
using System.Collections.Concurrent;

namespace MDA.MessageBus
{
    public class DisruptorMessageQueueService : IMessageQueueService
    {
        private readonly ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>> _queues = new ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>>();

        private readonly IServiceProvider _serviceProvider;

        public DisruptorMessageQueueService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public void Start()
        {
            var disruptor = new Disruptor<DisruptorTransportEvent>(() => new DisruptorTransportEvent(), 1024);

            disruptor.HandleEventsWith(new DisruptorTransportEventHandler());
            disruptor.Start();

            _queues[MessagePartitionKeys.GlobalPartitionKey] = disruptor;
        }

        public void Enqueue(IMessage message) => DoEnqueue(message);

        public void Enqueue<TPayload>(IMessage<TPayload> message) => DoEnqueue(message);

        public void Stop()
        {
            foreach (var queue in _queues)
            {
                queue.Value.Shutdown(TimeSpan.Zero);
            }
        }

        public void DoEnqueue(IMessage message)
        {
            var partitionKey = message.PartitionKey;

            _queues.AddOrUpdate(partitionKey, (key) =>
                {
                    var newDisruptor = new Disruptor<DisruptorTransportEvent>(() => new DisruptorTransportEvent(), 1024);

                    newDisruptor.Start();

                    return newDisruptor;
                }, (key, oldValue) => oldValue);

            var ringBuffer = _queues[partitionKey].RingBuffer;
            var publishable = ringBuffer.TryNext(out var sequence);
            if (!publishable)
            {
                throw new MessageRingBufferNotAvailableException("Cannot get available sequence from message ring buffer.");
            }

            var transportMessage = ringBuffer[sequence];

            transportMessage.Sequence = sequence;
            transportMessage.Message = message;
            transportMessage.ServiceProvider = _serviceProvider;

            ringBuffer.Publish(sequence);
        }
    }

    public class DisruptorTransportEvent
    {
        public long Sequence { get; set; }

        public IMessage Message { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
    }
}
