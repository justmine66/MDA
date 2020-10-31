using Disruptor.Dsl;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorAsyncMessageQueueService : IAsyncMessageQueueService
    {
        private readonly ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>> _queues = new ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>>();

        private readonly IServiceProvider _serviceProvider;

        public DisruptorAsyncMessageQueueService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        public MessageBusClientNames Name => MessageBusClientNames.Disruptor;

        public async Task StartAsync(CancellationToken token = default)
        {
            _queues[MessagePartitionKeys.GlobalPartitionKey] = CreateAndStartDisruptor();

            await Task.CompletedTask;
        }

        public async Task EnqueueAsync(IMessage message, CancellationToken token = default)
        {
            DoEnqueue(message);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token = default)
        {
            foreach (var queue in _queues)
            {
                queue.Value.Shutdown(TimeSpan.Zero);
            }

            await Task.CompletedTask;
        }

        public void DoEnqueue(IMessage message)
        {
            var partitionKey = message.PartitionKey;

            _queues.AddOrUpdate(partitionKey, (key) => CreateAndStartDisruptor(), (key, oldValue) => oldValue);

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

        private Disruptor<DisruptorTransportEvent> CreateAndStartDisruptor()
        {
            var newDisruptor = new Disruptor<DisruptorTransportEvent>(() => new DisruptorTransportEvent(), 1024);

            newDisruptor.HandleEventsWith(new DisruptorTransportEventHandler());
            newDisruptor.Start();

            return newDisruptor;
        }
    }

    public class DisruptorTransportEvent
    {
        public long Sequence { get; set; }

        public IMessage Message { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
    }
}
