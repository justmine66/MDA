using Orleans;
using System;
using System.Threading.Tasks;

namespace Grain.interfaces.Transactions
{
    public interface IATMGrain : IGrainWithIntegerKey
    {
        [Transaction(TransactionOption.RequiresNew)]
        Task Transfer(Guid fromAccount, Guid toAccount, uint amountToTransfer);
    }
}
