﻿using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    public class ApplicationCommandProcessor<TApplicationCommand> :
        IMessageHandler<TApplicationCommand>,
        IAsyncMessageHandler<TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        private readonly IApplicationCommandContext _context;

        public ApplicationCommandProcessor(IApplicationCommandContext context) 
            => _context = context;

        public void Handle(TApplicationCommand message)
        {
            var handler = _context
                .ServiceProvider
                .GetService<IApplicationCommandHandler<TApplicationCommand>>();

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            handler.OnApplicationCommand(_context, message);
        }

        public async Task HandleAsync(TApplicationCommand message, CancellationToken token = default)
        {
            var handler = _context
                .ServiceProvider
                .GetService<IAsyncApplicationCommandHandler<TApplicationCommand>>();

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            await handler.OnApplicationCommandAsync(_context, message, token);
        }
    }
}