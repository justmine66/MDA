using System;
using MDA.Domain.Models;
using MDA.MessageBus;

namespace MDA.Domain.Commands
{
    public class DomainCommandPublisher : IDomainCommandPublisher
    {
        private readonly IMessagePublisher _messagePublisher;

        public DomainCommandPublisher(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        public void Publish<TDomainCommand>(TDomainCommand command) where TDomainCommand : IDomainCommand
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var message = new DomainCommandTransportMessage(command);

            _messagePublisher.Publish(message);
        }

        public void Publish<TAggregateRoot, TDomainCommand>(TDomainCommand command) where TAggregateRoot : IAggregateRoot where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg>(IDomainCommandFiller<TDomainCommand, TArg> filler, TArg arg) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg>(Action<TDomainCommand, TArg> filler, TArg arg) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(IDomainCommandFiller<TDomainCommand, TArg1, TArg2> filler, TArg1 arg1, TArg2 arg2) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(Action<TDomainCommand, TArg1, TArg2> filler, TArg1 arg1, TArg2 arg2) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(IDomainCommandFiller<TDomainCommand, TArg1, TArg2, TArg3> filler, TArg1 arg1, TArg2 arg2, TArg3 arg3) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(Action<TDomainCommand, TArg1, TArg2, TArg3> filler, TArg1 arg1, TArg2 arg2, TArg3 arg3) where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }
    }
}
