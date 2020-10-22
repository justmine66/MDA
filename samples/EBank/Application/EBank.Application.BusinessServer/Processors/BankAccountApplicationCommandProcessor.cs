using System.Threading;
using System.Threading.Tasks;
using EBank.Application.Commands.Accounts;
using EBank.Domain.Commands.Accounts;
using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;

namespace EBank.Application.BusinessServer.Processors
{
    public class BankAccountApplicationCommandProcessor : 
        IAsyncApplicationCommandHandler<OpenBankAccountApplicationCommand>
    {
        private readonly IBankAccountRepository _accountIndex;

        public BankAccountApplicationCommandProcessor(IBankAccountRepository repository)
        {
            _accountIndex = repository;
        }

        public async Task OnApplicationCommandAsync(
            IApplicationCommandContext context,
            OpenBankAccountApplicationCommand appCommand,
            CancellationToken token = default)
        {
            var hadAccountName = await _accountIndex.HadAccountNameAsync(appCommand.AccountName).ConfigureAwait(false);
            if (hadAccountName)
            {
                throw new BankAccountDomainException("账户名已经存在。");
            }

            var domainCommand = new OpenAccountDomainCommand(
                appCommand.AccountId,
                appCommand.AccountName,
                appCommand.Bank,
                appCommand.InitialBalance);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
