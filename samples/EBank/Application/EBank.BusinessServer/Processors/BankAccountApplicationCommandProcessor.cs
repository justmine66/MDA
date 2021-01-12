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
                appCommand.InitialBalance ?? 0)
            {
                ApplicationCommandId = appCommand.Id,
                ApplicationCommandType = appCommand.GetType().FullName
            };

            context.DomainCommandPublisher.Publish(domainCommand);
        }

        public void OnApplicationCommand(
            IApplicationCommandContext context,
            ChangeAccountNameApplicationCommand command)
        {
            var domainCommand = new ChangeAccountNameDomainCommand(command.AccountId, command.AccountName);

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
