using MDA.Infrastructure.Utils;
using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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

        public void Handle(TApplicationCommand command)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IApplicationCommandHandler<TApplicationCommand>>();

                PreConditions.NotNull(handler,nameof(handler));

                handler.OnApplicationCommand(_context, command);
            }
        }

        public async Task HandleAsync(TApplicationCommand command, CancellationToken token = default)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IAsyncApplicationCommandHandler<TApplicationCommand>>();

                PreConditions.NotNull(handler, nameof(handler));

                await handler.OnApplicationCommandAsync(_context, command, token);
            }
        }
    }
}
