using System.Threading.Tasks;

namespace EBank.Domain.Models.Accounts
{
    public interface IBankAccountRepository
    {
        Task<bool> HadAccountNameAsync(string name);
    }
}
