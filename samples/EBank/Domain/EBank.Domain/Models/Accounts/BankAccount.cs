﻿using EBank.Domain.Commands.Accounts;
using EBank.Domain.Events.Accounts;
using EBank.Domain.Models.Accounts.Primitives;
using EBank.Domain.Models.Primitives;
using EBank.Domain.Models.Transferring;
using EBank.Domain.Notifications.Accounts;
using MDA.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EBank.Domain.Models.Accounts
{
    /// <summary>
    /// 表示一个银行账户聚合根，内存状态。
    /// </summary>
    public partial class BankAccount
    {
        /// <summary>
        /// 在途账户交易记录
        /// </summary>
        private IDictionary<AccountTransactionId, AccountTransaction> _transactionsInFlight;

        /// <summary>
        /// 账户名
        /// </summary>
        public BankAccountName Name { get; private set; }

        /// <summary>
        /// 余额
        /// </summary>
        public Money Balance { get; private set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public BankName Bank { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public BankAccountStatus Status { get; private set; }

        /// <summary>
        /// 添加一笔在途账户交易
        /// </summary>
        /// <param name="transaction">账户交易信息</param>
        public void AddAccountTransaction(AccountTransaction transaction)
        {
            if (_transactionsInFlight == null)
            {
                _transactionsInFlight = new Dictionary<AccountTransactionId, AccountTransaction>();
            }

            _transactionsInFlight[transaction.Id] = transaction;
        }

        /// <summary>
        /// 移除一笔在途账户交易
        /// </summary>
        /// <param name="transactionId">交易号</param>
        /// <param name="transaction">已删除的账户交易</param>
        public bool TryRemoveAccountTransaction(long transactionId, out AccountTransaction transaction)
        {
            if (_transactionsInFlight == null)
            {
                transaction = null;

                return false;
            }

            return _transactionsInFlight.TryGetValue(transactionId, out transaction) &&
                _transactionsInFlight.Remove(transactionId);
        }

        /// <summary>
        /// 可用余额。
        /// 计算规则：可用余额 = 账户余额 + 在途收入总额 - 在途支出总额。
        /// </summary>
        public Money AvailableBalance => Balance + InAmountInFlight - OutAmountInFlight;

        /// <summary>
        /// 在途收入总额
        /// </summary>
        public Money InAmountInFlight
        {
            get
            {
                return _transactionsInFlight?.Values
                                           .Where(it => it.FundDirection == AccountFundDirection.In)
                                           .Sum(it => it.Money.Amount) ?? 0;
            }
        }

        /// <summary>
        /// 在途支出总额
        /// </summary>
        public Money OutAmountInFlight
        {
            get
            {
                return _transactionsInFlight?.Values
                           .Where(it => it.FundDirection == AccountFundDirection.Out)
                           .Sum(it => it.Money.Amount) ?? 0;
            }
        }
    }

    /// <summary>
    /// 表示一个账户聚合根，处理业务。
    /// </summary>
    public partial class BankAccount : EventSourcedAggregateRoot<BankAccountId>
    {
        #region [ Handler Domain Commands ]

        /// <summary>
        /// 开户
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(OpenAccountDomainCommand command)
        {
            var @event = new AccountOpenedDomainEvent(command.AccountName, command.Bank, command.InitialBalance);

            ApplyDomainEvent(@event);
        }

        /// <summary>
        /// 变更账户名
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ChangeAccountNameDomainCommand command)
        {
            var evt = new AccountNameChangedDomainEvent(command.AccountName);

            ApplyDomainEvent(evt);
        }

        /// <summary>
        /// 验证存款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateDepositTransactionDomainCommand command)
        {
            // 1. 业务预检
            if (!Name.Equals(command.AccountName))
            {
                PublishDomainNotification(new DepositTransactionValidateFailedDomainNotification(command.TransactionId, "账户名不正确。"));

                return;
            }

            if (!Bank.Equals(command.Bank))
            {
                PublishDomainNotification(new DepositTransactionValidateFailedDomainNotification(command.TransactionId, "开户行不匹配。"));

                return;
            }

            // 2. 处理业务
            AddAccountTransaction(new AccountTransaction(command.TransactionId, command.Money, AccountFundDirection.In));
            PublishDomainNotification(new DepositTransactionValidatedDomainNotification(command.TransactionId, command.Money));
        }

        /// <summary>
        /// 提交存款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(SubmitDepositTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var inAmountInFlight = InAmountInFlight;
            var outAmountInFlight = OutAmountInFlight;

            // 1. 业务预检
            if (!TryRemoveAccountTransaction(command.TransactionId, out var transaction))
            {
                throw new BankAccountDomainException($"在账户[{Id},{Name},{Bank}]中，没有找到可提交的在途存款交易: {transactionId}.");
            }

            // 2. 处理业务
            ApplyDomainEvent(new DepositTransactionSubmittedDomainEvent(transaction.Id, transaction.Money, Balance, inAmountInFlight, outAmountInFlight));
        }

        /// <summary>
        /// 验证取款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateWithdrawTransactionDomainCommand command)
        {
            // 1. 业务预检
            if (!Name.Equals(command.AccountName))
            {
                PublishDomainNotification(new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, "账户名不正确。"));

                return;
            }

            if (!Bank.Equals(command.Bank))
            {
                PublishDomainNotification(new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, "开户行不匹配。"));

                return;
            }

            if (AvailableBalance < command.Money)
            {
                var notification = new WithdrawTransactionValidateFailedDomainNotification(command.TransactionId, $"余额不足，可用余额: {AvailableBalance.ToShortString()}, 取款金额: {command.Money.ToShortString()}。");

                PublishDomainNotification(notification);

                return;
            }

            // 2. 处理业务
            AddAccountTransaction(new AccountTransaction(command.TransactionId, command.Money, AccountFundDirection.Out));
            PublishDomainNotification(new WithdrawTransactionValidatedDomainNotification(command.TransactionId, command.Money));
        }

        /// <summary>
        /// 提交取款交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(SubmitWithdrawTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var inAmountInFlight = InAmountInFlight;
            var outAmountInFlight = OutAmountInFlight;

            // 1. 业务预检
            if (!TryRemoveAccountTransaction(command.TransactionId, out var transaction))
            {
                throw new BankAccountDomainException($"在账户[{Id},{Name},{Bank}]中，没有找到在途取款交易: {transactionId}.");
            }

            // 2. 处理业务
            ApplyDomainEvent(new WithdrawTransactionSubmittedDomainEvent(transaction.Id, transaction.Money, Balance, inAmountInFlight, outAmountInFlight));
        }

        /// <summary>
        /// 验证转账交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(ValidateTransferTransactionDomainCommand command)
        {
            var amount = command.Money;
            var account = command.Account;
            var accountType = account.AccountType;
            var accountTypeString = accountType == TransferAccountType.Source ? "源" : "目标";

            // 1. 业务预检
            if (!Id.Equals(command.AggregateRootId))
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, $"{accountTypeString}账户的账户号不正确。"));

                return;
            }

            if (!Name.Equals(account.Name))
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, $"{accountTypeString}账户的账户名不正确。"));

                return;
            }

            if (!Bank.Equals(account.Bank))
            {
                PublishDomainNotification(new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, $"{accountTypeString}账户的开户行不匹配。"));

                return;
            }

            var availableBalance = AvailableBalance;

            if (accountType == TransferAccountType.Source && availableBalance < amount)
            {
                var notification = new TransferTransactionValidateFailedDomainNotification(command.TransactionId, accountType, $"源账户余额不足，可用余额: {availableBalance.ToShortString()}, 转账金额: {amount.ToShortString()}。");

                PublishDomainNotification(notification);

                return;
            }

            // 2. 处理业务
            switch (accountType)
            {
                case TransferAccountType.Source:
                    AddAccountTransaction(new AccountTransaction(command.TransactionId, command.Money, AccountFundDirection.Out));
                    break;
                case TransferAccountType.Sink:
                    AddAccountTransaction(new AccountTransaction(command.TransactionId, command.Money, AccountFundDirection.In));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PublishDomainNotification(new TransferTransactionValidatedDomainNotification(command.TransactionId, command.Money, accountType));
        }

        /// <summary>
        /// 释放转账在途交易
        /// </summary>
        /// <param name="context"></param>
        /// <param name="command"></param>
        public void OnDomainCommand(AggregateRootMessagingContext context, FreeTransferAccountTransactionDomainCommand command)
        {
            var logger = context.ServiceProvider.GetService<ILogger<BankAccount>>();

            if (TryRemoveAccountTransaction(command.TransactionId, out var transaction))
            {
                logger.LogInformation($"已释放转账在途交易: [{command.TransactionId},{transaction.Money.ToShortString()},{transaction.FundDirection}], 账户: [{Id},{Name},{Bank}]");
            }
            else
            {
                logger.LogWarning($"没有待释放的转账在途交易: {command.TransactionId}, 账户: [{Id},{Name},{Bank}]");
            }
        }

        /// <summary>
        /// 提交转账交易
        /// </summary>
        /// <param name="command">领域命令</param>
        public void OnDomainCommand(SubmitTransferTransactionDomainCommand command)
        {
            var transactionId = command.TransactionId;
            var inAmountInFlight = InAmountInFlight;
            var outAmountInFlight = OutAmountInFlight;

            if (!TryRemoveAccountTransaction(command.TransactionId, out var transaction))
            {
                throw new BankAccountDomainException($"在账户[{Id},{Name},{Bank}]中，没有找到在途转账交易: {transactionId}.");
            }

            var fundDirection = transaction.FundDirection;

            switch (fundDirection)
            {
                case AccountFundDirection.In:
                    ApplyDomainEvent(new TransferTransactionSubmittedDomainEvent(transaction.Id, transaction.Money, TransferAccountType.Sink, Balance, inAmountInFlight, outAmountInFlight));
                    break;
                case AccountFundDirection.Out:
                    ApplyDomainEvent(new TransferTransactionSubmittedDomainEvent(transaction.Id, transaction.Money, TransferAccountType.Source, Balance, inAmountInFlight, outAmountInFlight));
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

        public void OnDomainEvent(AccountNameChangedDomainEvent @event)
        {
            Name = @event.AccountName;
        }

        public void OnDomainEvent(DepositTransactionSubmittedDomainEvent @event)
        {
            Balance += @event.Money;
        }

        public void OnDomainEvent(WithdrawTransactionSubmittedDomainEvent @event)
        {
            Balance -= @event.Money;
        }

        public void OnDomainEvent(TransferTransactionSubmittedDomainEvent @event)
        {
            switch (@event.AccountType)
            {
                case TransferAccountType.Source:
                    Balance -= @event.Money;
                    break;
                case TransferAccountType.Sink:
                    Balance += @event.Money;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
