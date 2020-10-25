using MDA.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandPublisher : IApplicationCommandPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public DefaultApplicationCommandPublisher(IMessagePublisher messagePublisher)
            => _messagePublisher = messagePublisher;

        public void Publish(IApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _messagePublisher.Publish(command);
        }

        public async Task PublishAsync(IApplicationCommand command, CancellationToken token = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            await _messagePublisher.PublishAsync(command, token);
        }
    }
}
