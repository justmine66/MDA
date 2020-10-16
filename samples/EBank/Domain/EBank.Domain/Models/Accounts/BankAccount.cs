using EBank.Domain.Commands.Accounts;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Notifications;
using MDA.Domain.Events;
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
        private readonly IDictionary<long, AccountTransaction> _transactions;

        public BankAccount(
            long id,
            string name,
            decimal balance,
            string bank) : base(id)
        {
            Name = name;
            Balance = balance;
            Bank = bank;
            Status = BankAccountStatus.Activated;
            _transactions = new Dictionary<long, AccountTransaction>();
        }

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
        /// 添加一笔账户交易
        /// </summary>
        /// <param name="transaction"></param>
        public void AddAccountTransaction(AccountTransaction transaction) => _transactions[transaction.Id] = transaction;

        /// <summary>
        /// 添加一笔账户交易
        /// </summary>
        /// <param name="transactionId"></param>
        public void CommitAccountTransaction(long transactionId)
        {
            var transaction = _transactions[transactionId];
            if (transaction == null)
            {
                throw new BankAccountDomainException($"在账户: {Id}中，没有找到交易: {transactionId}.");
            }

            IDomainEvent @event;

            switch (transaction.Type)
            {
                case AccountTransactionType.Withdraw:
                    @event = new WithdrawAccountTransactionCompletedDomainEvent(transaction.Id, transaction.Amount);
                    break;
                case AccountTransactionType.Deposit:
                    @event = new DepositAccountTransactionCompletedDomainEvent(transaction.Id, transaction.Amount);
                    break;
                default:
                    throw new BankAccountDomainException($"不支持的存款账户交易类型: {transaction.Type}。");
            }

            ApplyDomainEvent(@event);
        }

        /// <summary>
        /// 移除一笔账户交易
        /// </summary>
        /// <param name="transactionId"></param>
        public void RemoveAccountTransaction(long transactionId)
        {
            var removed = _transactions.Remove(transactionId);
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
                var inFlightInAmount = _transactions.Values.Where(it => it.Type == AccountTransactionType.Deposit).Sum(it => it.Amount);
                // 在途支出金额
                var inFlightOutAmount = _transactions.Values.Where(it => it.Type == AccountTransactionType.Withdraw).Sum(it => it.Amount);

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

            var @event = new AccountOpenedDomainEvent(command.AggregateRootId, command.AccountName, command.Bank, command.InitialBalance);

            ApplyDomainEvent(@event);
        }

        /// <summary>
        /// 发起存款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(StartDepositAccountTransactionDomainCommand command)
        {
            // 1. 参数预检
            PreConditions.NotNullOrWhiteSpace<StartDepositAccountTransactionDomainCommand>(nameof(command.AccountName), command.AccountName);
            PreConditions.GreaterThanOrEqual<StartDepositAccountTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<StartDepositAccountTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Maximum);
            PreConditions.NotNullOrWhiteSpace<StartDepositAccountTransactionDomainCommand>(nameof(command.Bank), command.Bank);
            PreConditions.GreaterThanZero<StartDepositAccountTransactionDomainCommand>(nameof(command.Amount), command.Amount);

            // 2. 业务预检
            if (Id != command.AccountId)
            {
                throw new BankAccountDomainException("存款失败，账户号不正确。");
            }
            if (Name != command.AccountName)
            {
                throw new BankAccountDomainException("存款失败，账户名不正确。");
            }
            if (Bank != command.Bank)
            {
                throw new BankAccountDomainException("存款失败，开户行不匹配。");
            }

            IDomainEvent @event;
            var stage = command.TransactionStage;

            switch (stage)
            {
                case AccountTransactionStage.Preparation:
                    @event = new DepositAccountTransactionReadiedDomainEvent(command.TransactionId, command.Amount);
                    break;
                case AccountTransactionStage.Commitment:
                    @event = new DepositAccountTransactionCompletedDomainEvent(command.TransactionId, command.Amount);
                    break;
                default:
                    throw new BankAccountDomainException($"不支持的存款账户交易阶段: {stage}。");
            }

            ApplyDomainEvent(@event);
        }

        /// <summary>
        /// 发起取款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(StartWithdrawAccountTransactionDomainCommand command)
        {
            // 1. 参数预检
            PreConditions.NotNullOrWhiteSpace<StartDepositAccountTransactionDomainCommand>(nameof(command.AccountName), command.AccountName);
            PreConditions.GreaterThanOrEqual<StartDepositAccountTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.LessThanOrEqual<StartDepositAccountTransactionDomainCommand>($"{nameof(command.AccountName)} length", command.AccountName.Length, DomainRules.PreConditions.Account.Name.Length.Minimum);
            PreConditions.NotNullOrWhiteSpace<StartDepositAccountTransactionDomainCommand>(nameof(command.Bank), command.Bank);
            PreConditions.GreaterThanZero<StartDepositAccountTransactionDomainCommand>(nameof(command.Amount), command.Amount);

            // 2. 业务预检
            if (Id != command.AccountId)
            {
                throw new BankAccountDomainException("取款失败，账户号不正确。");
            }
            if (Name != command.AccountName)
            {
                throw new BankAccountDomainException("取款失败，账户名不正确。");
            }
            if (Bank != command.Bank)
            {
                throw new BankAccountDomainException("取款失败，开户行不匹配。");
            }
            if (AvailableBalance < command.Amount)// 余额不足
            {
                var notification = new TransactionAccountInsufficientBalanceDomainNotification(command.TransactionId, command.Amount, AvailableBalance, AccountTransactionType.Deposit, AccountTransactionStage.Commitment);

                PublishDomainNotification(notification);

                return;
            }

            IDomainEvent @event;
            var stage = command.TransactionStage;

            switch (stage)
            {
                case AccountTransactionStage.Preparation:
                    @event = new WithdrawAccountTransactionReadiedDomainEvent(command.TransactionId, command.Amount);
                    break;
                case AccountTransactionStage.Commitment:
                    @event = new WithdrawAccountTransactionCompletedDomainEvent(command.TransactionId, command.Amount);
                    break;
                default:
                    throw new BankAccountDomainException($"不支持的取款账户交易阶段: {stage}。");
            }

            ApplyDomainEvent(@event);
        }

        #endregion

        #region [ Handler Domain Events ]

        public void OnDomainEvent(AccountOpenedDomainEvent @event)
        {
            var account = new BankAccount(@event.AggregateRootId, @event.AccountName, @event.InitialBalance, @event.Bank);
        }

        public void OnDomainEvent(DepositAccountTransactionReadiedDomainEvent @event)
        {
            var transaction = new AccountTransaction(@event.TransactionId, @event.Amount, AccountTransactionType.Deposit, AccountTransactionStage.Preparation);

            AddAccountTransaction(transaction);
        }

        public void OnDomainEvent(DepositAccountTransactionCompletedDomainEvent @event)
        {
            Balance += @event.Amount;
        }

        public void OnDomainEvent(WithdrawAccountTransactionReadiedDomainEvent @event)
        {
            var transaction = new AccountTransaction(@event.TransactionId, @event.Amount, AccountTransactionType.Withdraw, AccountTransactionStage.Preparation);

            AddAccountTransaction(transaction);
        }

        public void OnDomainEvent(WithdrawAccountTransactionCompletedDomainEvent @event)
        {
            Balance -= @event.Amount;
        }

        #endregion
    }
}
