using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Notifications.Depositing;
using EBank.Application.Notifications.Transferring;
using EBank.Application.Querying;
using EBank.Domain.Commands.Accounts;
using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace EBank.Application.BusinessServer.Processors.Accounts
{
    public class BankAccountApplicationCommandProcessor :
        IAsyncApplicationCommandHandler<OpenBankAccountApplicationCommand>,
        IAsyncApplicationCommandHandler<ValidateTransferTransactionApplicationCommand>,
        IAsyncApplicationCommandHandler<ValidateDepositTransactionApplicationCommand>,
        IApplicationCommandHandler<StartDepositAccountTransactionApplicationCommand>,
        IApplicationCommandHandler<StartWithdrawAccountTransactionApplicationCommand>
    {
        private readonly IBankAccountRepository _accountIndex;
        private readonly IBankAccountQueryService _accountQuery;

        public BankAccountApplicationCommandProcessor(
            IBankAccountRepository repository,
            IBankAccountQueryService accountQuery)
        {
            _accountIndex = repository;
            _accountQuery = accountQuery;
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

        public async Task OnApplicationCommandAsync(IApplicationCommandContext context, ValidateTransferTransactionApplicationCommand command, CancellationToken token = default)
        {
            var accountToValidate = command.Account;
            var account = await _accountQuery.GetAccountAsync(accountToValidate.Id);
            if (account == null)
            {
                var failed = new TransferTransactionAccountValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Account = command.Account,
                    AccountType = command.AccountType,
                    Reason = "账户不存在"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            if (account.Name != accountToValidate.Name)
            {
                var failed = new TransferTransactionAccountValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Account = command.Account,
                    AccountType = command.AccountType,
                    Reason = "账户名不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            if (account.Bank != accountToValidate.Bank)
            {
                var failed = new TransferTransactionAccountValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Account = command.Account,
                    AccountType = command.AccountType,
                    Reason = "开户行不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            var passed = new TransferTransactionAccountValidatePassedApplicationNotification()
            {
                TransactionId = command.TransactionId,
                AccountType = command.AccountType
            };

            await context.ApplicationNotificationPublisher.PublishAsync(passed, token);
        }

        public async Task OnApplicationCommandAsync(IApplicationCommandContext context, ValidateDepositTransactionApplicationCommand command, CancellationToken token = default)
        {
            var account = await _accountQuery.GetAccountAsync(command.AccountId);
            if (account == null)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "账户不存在"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            if (account.Name != command.AccountName)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "账户名不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            if (account.Bank != command.Bank)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "开户行不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed, token);
                return;
            }

            var passed = new TransferTransactionAccountValidatePassedApplicationNotification()
            {
                TransactionId = command.TransactionId
            };

            await context.ApplicationNotificationPublisher.PublishAsync(passed, token);
        }

        public void OnApplicationCommand(
            IApplicationCommandContext context, 
            StartDepositAccountTransactionApplicationCommand appCommand)
        {
            var domainCommand = new StartDepositAccountTransactionDomainCommand
            {
                AggregateRootId = appCommand.AccountId,
                AccountName = appCommand.AccountName,
                Bank = appCommand.Bank,
                Amount = appCommand.Amount,
                TransactionStage = appCommand.TransactionStage
            };

            context.DomainCommandPublisher.Publish(domainCommand);
        }

        public void OnApplicationCommand(
            IApplicationCommandContext context, 
            StartWithdrawAccountTransactionApplicationCommand appCommand)
        {
            var domainCommand = new StartWithdrawAccountTransactionDomainCommand
            {
                AggregateRootId = appCommand.AccountId,
                AccountName = appCommand.AccountName,
                Bank = appCommand.Bank,
                Amount = appCommand.Amount,
                TransactionStage = appCommand.TransactionStage
            };

            context.DomainCommandPublisher.Publish(domainCommand);
        }
    }
}
