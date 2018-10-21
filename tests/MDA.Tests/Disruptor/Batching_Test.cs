using MDA.Disruptor;
using MDA.Disruptor.dsl;
using MDA.Tests.Disruptor.Support;
using System.Collections.Generic;
using System.Threading;

namespace MDA.Tests.Disruptor
{
    public class Batching_Test
    {
        private readonly ProducerType _producerType;

        public Batching_Test(ProducerType producerType)
        {
            _producerType = producerType;
        }

        public static IEnumerable<object[]> GenerateData()
        {
            yield return new object[] { ProducerType.Single };
            yield return new object[] { ProducerType.Multi };
        }

        private class ParallelEventHandler : IEventHandler<LongEvent>
        {
            private readonly long _mask;
            private readonly long _ordinal;
            private readonly int _batchSize = 10;

            private long _eventCount;
            private long _batchCount;
            private long _publishedValue;
            private long _tempValue;
            private long _processed;

            private ParallelEventHandler(long mask, long ordinal)
            {
                _mask = mask;
                _ordinal = ordinal;
            }

            public void OnEvent(LongEvent @event, long sequence, bool endOfBatch)
            {
                if ((sequence & _mask) == _ordinal)
                {
                    _eventCount++;
                    _tempValue = @event.Value;
                }

                if (endOfBatch || ++_batchCount >= _batchSize)
                {
                    _publishedValue = _tempValue;
                    _batchCount = 0;
                }
                else
                {
                    Thread.Sleep(0);
                }

                _processed = sequence;
            }
        }

        public void Should_Batch()
        {

        }
    }
}
