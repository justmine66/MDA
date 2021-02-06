using MDA.MessageBus;
using MDA.MessageBus.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        private readonly IApplicationCommandingContext _context;

        public ApplicationCommandProcessor(
            IApplicationCommandingContext context,
            ILogger<ApplicationCommandProcessor<TApplicationCommand>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Handle(TApplicationCommand command)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IApplicationCommandHandler<TApplicationCommand>>();
                if (handler == null)
                {
                    _logger.LogError($"The {typeof(IApplicationCommandHandler<TApplicationCommand>).FullName} no found.");

                    return;
                }

                _context.SetApplicationCommand(command);

                handler.OnApplicationCommand(_context, command);
            }
        }

        public async Task HandleAsync(TApplicationCommand command, CancellationToken token = default)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var handler = scope.ServiceProvider.GetService<IAsyncApplicationCommandHandler<TApplicationCommand>>();
                if (handler == null)
                {
                    _logger.LogError($"The {typeof(IAsyncApplicationCommandHandler<TApplicationCommand>).FullName} no found.");

                    return;
                }

                _context.SetApplicationCommand(command);

                await handler.OnApplicationCommandAsync(_context, command, token);
            }
        }
    }
}
