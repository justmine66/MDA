using EBank.Domain.Commands.Accounts;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Notifications;
using MDA.Domain.Models;
using MDA.Domain.Shared;
using System.Collections.Generic;
using System.Linq;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 表示一个银行账户聚合根，封装状态。
    /// </summary>
    public partial class BankAccount
    {
        /// <summary>
        /// 在途账户交易记录
        /// </summary>
        private IDictionary<long, AccountTransaction> _transactionsInFlight;

        /// <summary>
        /// 账户名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public BankAccountStatus Status { get; private set; }

        /// <summary>
        /// 添加一笔在途账户交易
        /// </summary>
        /// <param name="transaction">账户交易信息</param>
        public void AddAccountTransactionInFlight(AccountTransaction transaction)
        {
            if (_transactionsInFlight == null)
            {
                _transactionsInFlight = new Dictionary<long, AccountTransaction>();
            }

            _transactionsInFlight[transaction.Id] = transaction;
        }

        /// <summary>
        /// 移除一笔在途账户交易
        /// </summary>
        /// <param name="transactionId">交易号</param>
        public void RemoveAccountTransaction(long transactionId)
        {
            var removed = _transactionsInFlight.Remove(transactionId);
            if (!removed)
            {
                throw new BankAccountDomainException($"移除账户: {Id} 中交易: {transactionId}失败。");
            }
        }

        /// <summary>
        /// 可用余额。
        /// 计算规则：可用余额 = 账户余额 + 在途收入金额 - 在途支出金额。
        /// </summary>
        public decimal AvailableBalance
        {
            get
            {
                // 在途收入金额
                var inFlightInAmount = _transactionsInFlight?.Values
                                           .Where(it => it.FundDirection == AccountFundDirection.In)
                                           .Sum(it => it.Amount) ?? 0;
                // 在途支出金额
                var inFlightOutAmount = _transactionsInFlight?.Values
                                            .Where(it => it.FundDirection == AccountFundDirection.Out)
                                            .Sum(it => it.Amount) ?? 0;

                // 余额 - 在途占用
                return Balance + inFlightInAmount - inFlightOutAmount;
            }
        }
    }

    /// <summary>
    /// 表示一个账户聚合根，封装业务规则。
    /// </summary>
    public partial class BankAccount : EventSourcedAggregateRoot<long>
    {
        #region [ Handler Domain Commands ]

        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(OpenAccountDomainCommand command)
        {
            PreConditions.GreaterThanZero<OpenAccountDomainCommand>("Id", command.AggregateRootId);

            PreConditions.NotNullOrWhiteSpace<OpenAccountDomainCommand>(nameof(command.AccountName), command.AccountName);
            PreConditions.GreaterThanOrEqual<OpenAccountDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<OpenAccountDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length,
                DomainRules.PreConditions.Account.Name.Length.Maximum);

            PreConditions.GreaterThanZero<OpenAccountDomainCommand>(nameof(command.InitialBalance), command.InitialBalance);
            PreConditions.NotNullOrWhiteSpace<OpenAccountDomainCommand>(nameof(command.Bank), command.Bank);

            var @event = new AccountOpenedDomainEvent(command.AccountName, command.Bank, command.InitialBalance);

            ApplyDomainEvent(@event);
        }

        /// <summary>
        /// 验证存款交易信息
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateDepositTransactionDomainCommand command)
        {
            // 1. 参数预检
            PreConditions.NotNullOrWhiteSpace<ValidateDepositTransactionDomainCommand>(nameof(command.AccountName), command.AccountName);
            PreConditions.GreaterThanOrEqual<ValidateDepositTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<ValidateDepositTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Maximum);
            PreConditions.NotNullOrWhiteSpace<ValidateDepositTransactionDomainCommand>(nameof(command.Bank), command.Bank);

            // 2. 业务预检
            if (Id != command.AggregateRootId)
            {
                PublishDomainNotification(new DepositTransactionValidateFailedDomainNotification(command.TransactionId, "存款失败，账户号不正确。"));

                return;
            }
            if (Name != command.AccountName)
            {
                PublishDomainNotification(new DepositTransactionValidateFailedDomainNotification(command.TransactionId, "存款失败，账户名不正确。"));

                return;
            }
            if (Bank != command.Bank)
            {
                PublishDomainNotification(new DepositTransactionValidateFailedDomainNotification(command.TransactionId, "存款失败，开户行不匹配。"));

                return;
            }

            // 3. 业务操作
            ApplyDomainEvent(new DepositTransactionValidatedDomainEvent(command.TransactionId, command.Amount));
        }

        /// <summary>
        /// 提交存款交易
        /// </summary>
        /// <param name="command"></param>
        public void OnDomainCommand(SubmitDepositTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var transaction = _transactionsInFlight[transactionId];
            if (transaction == null)
            {
                throw new BankAccountDomainException($"在账户: {Id}中，没有找到在途存款交易: {transactionId}.");
            }

            ApplyDomainEvent(new WithdrawTransactionSubmittedDomainEvent(transaction.Id, transaction.Amount));
        }

        /// <summary>
        /// 发起取款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateWithdrawTransactionDomainCommand command)
        {
            // 1. 参数预检
            PreConditions.NotNullOrWhiteSpace<ValidateWithdrawTransactionDomainCommand>(nameof(command.AccountName), command.AccountName);
            PreConditions.GreaterThanOrEqual<ValidateWithdrawTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<ValidateWithdrawTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.NotNullOrWhiteSpace<ValidateWithdrawTransactionDomainCommand>(nameof(command.Bank), command.Bank);
            PreConditions.GreaterThanZero<ValidateWithdrawTransactionDomainCommand>(nameof(command.Amount), command.Amount);

            // 2. 业务预检
            if (Id != command.AggregateRootId)
            {
                PublishDomainNotification(new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, "取款失败，账户号不正确。"));

                return;
            }
            if (Name != command.AccountName)
            {
                PublishDomainNotification(new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, "取款失败，账户名不正确。"));

                return;
            }
            if (Bank != command.Bank)
            {
                PublishDomainNotification(new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, "取款失败，开户行不匹配。"));

                return;
            }
            if (AvailableBalance < command.Amount)
            {
                var notification = new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, $"取款失败，余额不足，可用余额: {AvailableBalance}, 取款金额: {command.Amount}。");

                PublishDomainNotification(notification);

                return;
            }

            // 3. 业务操作
            ApplyDomainEvent(new WithdrawTransactionValidatedDomainEvent(command.TransactionId, command.Amount));
        }

        /// <summary>
        /// 提交存款交易
        /// </summary>
        /// <param name="command"></param>
        public void OnDomainCommand(SubmitWithdrawTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var transaction = _transactionsInFlight[transactionId];
            if (transaction == null)
            {
                throw new BankAccountDomainException($"在账户: {Id}中，没有找到在途取款交易: {transactionId}.");
            }

            ApplyDomainEvent(new WithdrawTransactionSubmittedDomainEvent(transaction.Id, transaction.Amount));
        }

        /// <summary>
        /// 验证转账交易信息
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateTransferTransactionDomainCommand command)
        {
            var account = command.Account;
            var amount = command.Amount;
            var accountType = command.AccountType;

            // 1. 参数预检
            PreConditions.NotNullOrWhiteSpace<ValidateTransferTransactionDomainCommand>(nameof(account.Name), account.Name);
            PreConditions.GreaterThanOrEqual<ValidateTransferTransactionDomainCommand>($"{nameof(account.Name)} length", account.Name.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<ValidateTransferTransactionDomainCommand>($"{nameof(account.Name)} length", account.Name.Length, DomainRules.PreConditions.Account.Name.Length.Maximum);
            PreConditions.NotNullOrWhiteSpace<ValidateTransferTransactionDomainCommand>(nameof(account.Bank), account.Bank);

            // 2. 业务预检

            if (Id != command.AggregateRootId)
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, "账户号不正确。"));

                return;
            }
            if (Name != account.Name)
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, "账户名不正确。"));

                return;
            }
            if (Bank != account.Bank)
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, "开户行不匹配。"));

                return;
            }
            if (command.AccountType == TransferTransactionAccountType.Source &&
                AvailableBalance < amount)
            {
                var notification = new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, $"余额不足，可用余额: {AvailableBalance}, 转账金额: {amount}。");

                PublishDomainNotification(notification);

                return;
            }

            // 3. 业务操作
            ApplyDomainEvent(new TransferTransactionValidatedDomainEvent(command.TransactionId, command.Amount, accountType));
        }

        /// <summary>
        /// 提交转账交易
        /// </summary>
        /// <param name="command"></param>
        public void OnDomainCommand(SubmitTransferTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var transaction = _transactionsInFlight[transactionId];
            if (transaction == null)
            {
                throw new BankAccountDomainException($"在账户: {Id}中，没有找到在途转账交易: {transactionId}.");
            }

            var fundDirection = transaction.FundDirection;

            switch (fundDirection)
            {
                case AccountFundDirection.In:
                    ApplyDomainEvent(new TransferTransactionSubmittedDomainEvent(transaction.Id, transaction.Amount, TransferTransactionAccountType.Sink));
                    break;
                case AccountFundDirection.Out:
                    ApplyDomainEvent(new TransferTransactionSubmittedDomainEvent(transaction.Id, transaction.Amount, TransferTransactionAccountType.Source));
                    break;
                default:
                    throw new BankAccountDomainException($"不支持的账户资金流向: {fundDirection}。");
            }
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(AccountOpenedDomainEvent @event)
        {
            Id = @event.AggregateRootId;
            Name = @event.AccountName;
            Balance = @event.InitialBalance;
            Bank = @event.Bank;
            Status = BankAccountStatus.Activated;
        }

        public void OnDomainEvent(DepositTransactionValidatedDomainEvent @event)
        {
            var accountTransaction = new AccountTransaction(@event.TransactionId, @event.Amount, AccountFundDirection.In);

            AddAccountTransactionInFlight(accountTransaction);
        }

        public void OnDomainEvent(DepositTransactionSubmittedDomainEvent @event)
        {
            Balance += @event.Amount;
        }

        public void OnDomainEvent(WithdrawTransactionValidatedDomainEvent @event)
        {
            var transaction = new AccountTransaction(@event.TransactionId, @event.Amount, AccountFundDirection.Out);

            AddAccountTransactionInFlight(transaction);
        }

        public void OnDomainEvent(WithdrawTransactionSubmittedDomainEvent @event)
        {
            Balance -= @event.Amount;
        }

        public void OnDomainEvent(TransferTransactionValidatedDomainEvent @event)
        {
            switch (@event.AccountType)
            {
                case TransferTransactionAccountType.Source:
                    AddAccountTransactionInFlight(new AccountTransaction(@event.TransactionId, @event.Amount, AccountFundDirection.Out));
                    break;
                case TransferTransactionAccountType.Sink:
                    AddAccountTransactionInFlight(new AccountTransaction(@event.TransactionId, @event.Amount, AccountFundDirection.In));
                    break;
            }
        }

        public void OnDomainEvent(TransferTransactionSubmittedDomainEvent @event)
        {
            switch (@event.AccountType)
            {
                case TransferTransactionAccountType.Source:
                    Balance -= @event.Amount;
                    break;
                case TransferTransactionAccountType.Sink:
                    Balance += @event.Amount;
                    break;
            }
        }

        #endregion
    }
}
