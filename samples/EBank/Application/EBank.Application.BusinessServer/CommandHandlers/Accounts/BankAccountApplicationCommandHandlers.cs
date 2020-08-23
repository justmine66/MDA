using System.Threading;
using EBank.Application.Commands.Accounts;
using EBank.Application.Commands.Depositing;
using EBank.Application.Commands.Transferring;
using EBank.Application.Notifications.Depositing;
using EBank.Application.Notifications.Transferring;
using EBank.Application.Querying;
using EBank.Domain.Commands.Accounts;
using EBank.Domain.Models.Accounts;
using MDA.Application.Commands;
using MDA.Domain.Shared;
using System.Threading.Tasks;

namespace EBank.Application.BusinessServer.CommandHandlers.Accounts
{
    public class BankAccountApplicationCommandHandlers :
        IAsyncApplicationCommandHandler<OpenBankAccountApplicationCommand>,
        IAsyncApplicationCommandHandler<ValidateTransferTransactionApplicationCommand>,
        IAsyncApplicationCommandHandler<ValidateDepositTransactionApplicationCommand>,
        IApplicationCommandHandler<StartDepositAccountTransactionApplicationCommand>,
        IApplicationCommandHandler<StartWithdrawAccountTransactionApplicationCommand>
    {
        private readonly IBankAccountRepository _accountIndex;
        private readonly IBankAccountQueryService _accountQuery;

        public BankAccountApplicationCommandHandlers(
            IBankAccountRepository repository,
            IBankAccountQueryService accountQuery)
        {
            _accountIndex = repository;
            _accountQuery = accountQuery;
        }

        public async Task HandleAsync(IApplicationCommandContext context, OpenBankAccountApplicationCommand command, CancellationToken token = default)
        {
            var hadAccountName = await _accountIndex.HadAccountNameAsync(command.AccountName).ConfigureAwait(false);
            if (hadAccountName)
            {
                throw new BankAccountDomainException("账户名已经存在。");
            }

            // 命令需对外开发设置器：使用工厂委托填充领域命令
            context.DomainCommandPublisher
                .Publish<OpenAccountDomainCommand, OpenBankAccountApplicationCommand>(
                    (domainCommand, appCommand) =>
                    {
                        domainCommand.AggregateRootId = SnowflakeId.Default().NextId();
                        domainCommand.AccountName = appCommand.AccountName;
                        domainCommand.Bank = appCommand.Bank;
                        domainCommand.InitialBalance = appCommand.InitialBalance;
                    }, command);
        }

        public async Task HandleAsync(IApplicationCommandContext context, ValidateTransferTransactionApplicationCommand command, CancellationToken token = default)
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

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
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

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
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

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
                return;
            }

            var passed = new TransferTransactionAccountValidatePassedApplicationNotification()
            {
                TransactionId = command.TransactionId,
                AccountType = command.AccountType
            };

            await context.ApplicationNotificationPublisher.PublishAsync(passed);
        }

        public async Task HandleAsync(IApplicationCommandContext context, ValidateDepositTransactionApplicationCommand command, CancellationToken token = default)
        {
            var account = await _accountQuery.GetAccountAsync(command.AccountId);
            if (account == null)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "账户不存在"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
                return;
            }

            if (account.Name != command.AccountName)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "账户名不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
                return;
            }

            if (account.Bank != command.Bank)
            {
                var failed = new DepositTransactionValidateFailedApplicationNotification()
                {
                    TransactionId = command.TransactionId,
                    Reason = "开户行不正确"
                };

                await context.ApplicationNotificationPublisher.PublishAsync(failed);
                return;
            }

            var passed = new TransferTransactionAccountValidatePassedApplicationNotification()
            {
                TransactionId = command.TransactionId
            };

            await context.ApplicationNotificationPublisher.PublishAsync(passed);
        }

        public void Handle(IApplicationCommandContext context, StartDepositAccountTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(StartDepositAccountTransactionDomainCommandFiller.Instance, command);
        }

        public void Handle(IApplicationCommandContext context, StartWithdrawAccountTransactionApplicationCommand command)
        {
            context.DomainCommandPublisher.Publish(StartWithdrawAccountTransactionDomainCommandFiller.Instance, command);
        }
    }
}
