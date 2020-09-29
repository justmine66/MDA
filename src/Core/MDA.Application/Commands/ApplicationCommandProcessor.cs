using MDA.MessageBus;

namespace MDA.Application.Commands
{
    public class ApplicationCommandProcessor<TApplicationCommand> : IMessageHandler<TApplicationCommand>
        where TApplicationCommand : IApplicationCommand
    {
        private readonly IApplicationCommandContext _context;
        private readonly IApplicationCommandHandler<TApplicationCommand> _handler;

        public ApplicationCommandProcessor(
            IApplicationCommandContext context,
            IApplicationCommandHandler<TApplicationCommand> handler)
        {
            _context = context;
            _handler = handler;
        }

        public void Handle(TApplicationCommand message)
        {
            _handler.OnApplicationCommand(_context, message);
        }
    } 
}
