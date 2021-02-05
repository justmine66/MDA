using MDA.Domain;
using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using Microsoft.Extensions.Options;
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
            PreConditions.NotNull(command, nameof(command));

            command.Topic = _options.Topic;
            command.ReplyScheme = ApplicationCommandReplySchemes.None;

            _messagePublisher.Publish(command);
        }

        public async Task PublishAsync(IApplicationCommand command, CancellationToken token = default)
        {
            PreConditions.NotNull(command, nameof(command));

            command.Topic = _options.Topic;
            command.ReplyScheme = ApplicationCommandReplySchemes.None;

            await _messagePublisher.PublishAsync(command, token);
        }
    }
}
