﻿using Disruptor.Dsl;
using System;
using System.Collections.Concurrent;

namespace MDA.MessageBus.Disruptor
{
    public class DisruptorMessageQueueService : IMessageQueueService
    {
        private readonly ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>> _queues = new ConcurrentDictionary<long, Disruptor<DisruptorTransportEvent>>();
        private readonly IServiceProvider _serviceProvider;

        public DisruptorMessageQueueService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public MessageBusClientNames Name => MessageBusClientNames.Disruptor;

        public void Start()
        {
            _queues[MessagePartitionKeys.GlobalPartitionKey] = CreateAndStartDisruptor();
        }

        public void Enqueue(IMessage message)
        {
            DoEnqueue(message);
        }

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
}
