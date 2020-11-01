using MDA.MessageBus;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class DefaultApplicationCommandPublisher : IApplicationCommandPublisher
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ApplicationCommandOptions _options;

        public DefaultApplicationCommandPublisher(
            IMessagePublisher messagePublisher,
            IOptions<ApplicationCommandOptions> options)
        {
            _messagePublisher = messagePublisher;
            _options = options.Value;
        }

        public void Publish(IApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Topic = _options.Topic;

            _messagePublisher.Publish(command);
        }

        public async Task PublishAsync(IApplicationCommand command, CancellationToken token = default)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.Topic = _options.Topic;

            await _messagePublisher.PublishAsync(command, token);
        }
    }
}
