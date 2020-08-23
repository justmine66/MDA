using Disruptor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MDA.Domain.Commands
{
    public class DisruptorDomainCommandPublisher : IDomainCommandPublisher
    {
        private readonly IServiceProvider _services;

        public DisruptorDomainCommandPublisher(IServiceProvider services)
        {
            _services = services;
        }

        public void Publish<TDomainCommand>(TDomainCommand command) where TDomainCommand : IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg>(IDomainCommandFiller<TDomainCommand, TArg> translator, TArg arg)
            where TDomainCommand : class, IDomainCommand
        {
            if (translator == null)
            {
                throw new ArgumentNullException(nameof(translator));
            }

            var ringBuffer = _services.GetService<RingBuffer<TDomainCommand>>();
            if (ringBuffer == null)
            {
                throw new DomainCommandRingBufferUnRegisteredException($"Please register the {typeof(RingBuffer<TDomainCommand>).FullName} first to ioc container.");
            }

            var publishable = ringBuffer.TryNext(out var sequence);
            if (!publishable)
            {
                throw new DomainCommandRingBufferNotAvailableException("Cannot get available sequence from domain command ring buffer.");
            }

            try
            {
                var commandToPublish = ringBuffer[sequence];

                translator.TranslateTo(commandToPublish, arg);
            }
            catch (Exception e)
            {
                throw new DomainCommandPublishFailedException($"Publish domain command has a unknown exception: {e}.", e);
            }
            finally
            {
                ringBuffer.Publish(sequence);
            }
        }

        public void Publish<TDomainCommand, TArg>(Action<TDomainCommand, TArg> translator, TArg arg)
            where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(IDomainCommandFiller<TDomainCommand, TArg1, TArg2> translator, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2>(Action<TDomainCommand, TArg1, TArg2> translator, TArg1 arg1, TArg2 arg2)
            where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(IDomainCommandFiller<TDomainCommand, TArg1, TArg2, TArg3> translator, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }

        public void Publish<TDomainCommand, TArg1, TArg2, TArg3>(Action<TDomainCommand, TArg1, TArg2, TArg3> translator, TArg1 arg1, TArg2 arg2, TArg3 arg3)
            where TDomainCommand : class, IDomainCommand
        {
            throw new NotImplementedException();
        }
    }
}
