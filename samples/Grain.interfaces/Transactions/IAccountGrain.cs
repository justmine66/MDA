using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Transactions
{
    public interface IAccountGrain : IGrainWithGuidKey
    {
        [Transaction(TransactionOption.Required)]
        Task Withdraw(uint amount);

        [Transaction(TransactionOption.Required)]
        Task Deposit(uint amount);

        [Transaction(TransactionOption.Required)]
        Task<uint> GetBalance();
    }
}
