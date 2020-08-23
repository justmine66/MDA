using System;

namespace MDA.Domain.Commands
{
    public interface IDomainCommandPublisher
    {
        void Publish<TDomainCommand>(TDomainCommand command)
            where TDomainCommand : IDomainCommand;

        void Publish<TDomainCommand, TArg>(
            IDomainCommandFiller<TDomainCommand, TArg> translator, TArg arg)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg>(
            Action<TDomainCommand, TArg> translator, TArg arg)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2>(
            IDomainCommandFiller<TDomainCommand, TArg1, TArg2> translator, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2>(
            Action<TDomainCommand, TArg1, TArg2> translator, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2, TArg3>(
            IDomainCommandFiller<TDomainCommand, TArg1, TArg2, TArg3> translator, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2, TArg3>(
            Action<TDomainCommand, TArg1, TArg2, TArg3> translator, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand;
    }
}
