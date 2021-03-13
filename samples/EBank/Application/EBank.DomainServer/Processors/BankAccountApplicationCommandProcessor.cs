using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.DomainServer.Processors
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
            IApplicationCommandingContext context,
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
                appCommand.InitialBalance);

            context.PublishDomainCommand(domainCommand);
        }

        public void OnApplicationCommand(
            IApplicationCommandingContext context,
            ChangeAccountNameApplicationCommand appCommand)
        {
            var domainCommand = new ChangeAccountNameDomainCommand(appCommand.AccountId, appCommand.AccountName);

            context.PublishDomainCommand(domainCommand);
        }
    }
}
