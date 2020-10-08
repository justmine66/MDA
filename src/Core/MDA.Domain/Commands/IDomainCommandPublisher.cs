﻿using System;

namespace MDA.Domain.Commands
{
    public interface IDomainCommandPublisher
    {
        void Publish(IDomainCommand command);

        void Publish<TAggregateRootId>(IDomainCommand<TAggregateRootId> command);

        void Publish<TId, TAggregateRootId>(IDomainCommand<TId, TAggregateRootId> command);

        void Publish<TDomainCommand, TArg>(
        IDomainCommandFiller<TDomainCommand, TArg> filler, TArg arg)
        where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg>(
            Action<TDomainCommand, TArg> filler, TArg arg)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2>(
            IDomainCommandFiller<TDomainCommand, TArg1, TArg2> filler, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2>(
            Action<TDomainCommand, TArg1, TArg2> filler, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2, TArg3>(
            IDomainCommandFiller<TDomainCommand, TArg1, TArg2, TArg3> filler, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand;

        void Publish<TDomainCommand, TArg1, TArg2, TArg3>(
            Action<TDomainCommand, TArg1, TArg2, TArg3> filler, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand;
    }
}
