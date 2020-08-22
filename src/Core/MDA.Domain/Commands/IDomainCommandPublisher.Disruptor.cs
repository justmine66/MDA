using System;

namespace MDA.Domain.Commands
{
    public class DisruptorDomainCommandPublisher : IDomainCommandPublisher
    {
        public void Publish<TDomainCommand>(TDomainCommand command) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg>(IDomainCommandTranslator<TDomainCommand, TArg> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg>(Action<TDomainCommand, TArg> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(IDomainCommandTranslator<TDomainCommand, TArg1, TArg2> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(Action<TDomainCommand, TArg1, TArg2> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(IDomainCommandTranslator<TDomainCommand, TArg1, TArg2, TArg3> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(Action<TDomainCommand, TArg1, TArg2, TArg3> translator) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }
    }
}
