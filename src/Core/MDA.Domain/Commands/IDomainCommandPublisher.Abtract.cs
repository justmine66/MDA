using Disruptor;
using Microsoft.Extensions.Logging;

namespace MDA.Domain.Commands
{
    public abstract class DomainCommandPublisher 
    {
        protected ILogger Logger;
        protected RingBuffer<IDomainCommand> RingBuffer;

        protected DomainCommandPublisher(
            RingBuffer<IDomainCommand> ringBuffer,
            ILogger logger)
        {
            RingBuffer = ringBuffer;
            Logger = logger;
        }
        
    }
}
