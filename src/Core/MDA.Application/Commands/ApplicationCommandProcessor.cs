using MDA.MessageBus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MDA.Application.Commands
{
    [IgnoreMessageHandlerForDependencyInjection]
    public class ApplicationCommandProcessor<TApplicationCommand> :
        IMessageHandler<TApplicationCommand>,
        IAsyncMessageHandler<TApplicationCommand>
        where TApplicationCommand : class, IApplicationCommand
    {
        private readonly IApplicationCommandContext _context;

        public ApplicationCommandProcessor(IApplicationCommandContext context)
            => _context = context;

        public void Handle(TApplicationCommand message)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IApplicationCommandHandler<TApplicationCommand>>();

                if (handler == null)
                {
                    throw new ArgumentNullException(nameof(handler));
                }

                handler.OnApplicationCommand(_context, message);
            }
        }

        public async Task HandleAsync(TApplicationCommand message, CancellationToken token = default)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IAsyncApplicationCommandHandler<TApplicationCommand>>();

                if (handler == null)
                {
                    throw new ArgumentNullException(nameof(handler));
                }

                await handler.OnApplicationCommandAsync(_context, message, token);
            }
        }
    }
}
