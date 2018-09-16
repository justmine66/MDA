using Grain.interfaces.Transactions;
using Orleans.Transactions.Abstractions;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Transactions
{
    public class AccountGrain : Orleans.Grain, IAccountGrain
    {
        private readonly ITransactionalState<Balance> balance;

        public AccountGrain(
        [TransactionalState("balance")] ITransactionalState<Balance> balance)
        {
            this.balance = balance ?? throw new ArgumentNullException(nameof(balance));
        }

        Task IAccountGrain.Deposit(uint ammount)
        {
            this.balance.State.Value += ammount;
            this.balance.Save();
            return Task.CompletedTask;
        }

        Task<uint> IAccountGrain.GetBalance()
        {
            return Task.FromResult(this.balance.State.Value);
        }

        public Task Withdraw(uint amount)
        {
            this.balance.State.Value -= amount;
            this.balance.Save();
            return Task.CompletedTask;
        }
    }
}
