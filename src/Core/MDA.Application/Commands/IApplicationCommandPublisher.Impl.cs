using MDA.MessageBus;
using System;

namespace MDA.Application.Commands
{
    public class ApplicationCommandPublisher : IApplicationCommandPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public ApplicationCommandPublisher(IMessagePublisher messagePublisher)
            => _messagePublisher = messagePublisher;

        public void Publish(IApplicationCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            _messagePublisher.Publish(command);
        }
    }
}
