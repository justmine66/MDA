using EBank.Application.Commanding.Accounts;
using EBank.Domain.Commands.Accounts;
using EBank.Domain.Models.Accounts;
using EBank.Domain.Repositories.MySql;
using MDA.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.BusinessServer.Processors
{
    public class BankAccountApplicationCommandProcessor :
        IAsyncApplicationCommandHandler<OpenBankAccountApplicationCommand>,
        IApplicationCommandHandler<ChangeAccountNameApplicationCommand>
    {
        private readonly IBankAccountRepository _accountRepository;

        public BankAccountApplicationCommandProcessor(IBankAccountRepository repository)
        {
            _accountRepository = repository;
        }

        public async Task OnApplicationCommandAsync(
            IApplicationCommandContext context,
            OpenBankAccountApplicationCommand appCommand,
            CancellationToken token = default)
        {
            var hadAccountName = await _accountRepository.HadAccountNameAsync(appCommand.AccountName).ConfigureAwait(false);
            if (hadAccountName)
            {
                throw new BankAccountDomainException("账户名已经存在。");
            }

            var domainCommand = new OpenAccountDomainCommand(
                appCommand.AccountId,
                appCommand.AccountName,
                appCommand.Bank,
                appCommand.InitialBalance)
            {
                ApplicationCommandId = appCommand.Id,
                ApplicationCommandType = appCommand.GetType().FullName,
                ApplicationCommandReplyScheme = appCommand.ReplyScheme
            };

            context.DomainCommandPublisher.Publish(domainCommand);
        }

        public void OnApplicationCommand(
            IApplicationCommandContext context,
            ChangeAccountNameApplicationCommand appCommand)
        {
            var domainCommand = new ChangeAccountNameDomainCommand(appCommand.AccountId, appCommand.AccountName)
            {
                ApplicationCommandId = appCommand.Id,
                ApplicationCommandType = appCommand.GetType().FullName,
                ApplicationCommandReplyScheme = appCommand.ReplyScheme
            };

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
