using MDA.MessageBus;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Domain.Exceptions
{
    public class DefaultDomainExceptionPublisher : IDomainExceptionPublisher
    {
        private readonly DomainExceptionOptions _options;
        private readonly IMessagePublisher _messagePublisher;

        public DefaultDomainExceptionPublisher(
            IMessagePublisher messagePublisher,
            IOptions<DomainExceptionOptions> options)
        {
            _messagePublisher = messagePublisher;
            _options = options.Value;
        }

        public void Publish(IDomainExceptionMessage exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            exception.Topic = _options.Topic;

            _messagePublisher.Publish(exception);
        }

        public async Task PublishAsync(IDomainExceptionMessage exception, CancellationToken token = default)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            exception.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(exception, token);
        }
    }
}
