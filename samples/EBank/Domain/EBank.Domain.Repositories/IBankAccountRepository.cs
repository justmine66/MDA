using System.Threading.Tasks;

namespace EBank.Domain.Repositories
{
    public interface IBankAccountRepository
    {
        Task<bool> HadAccountNameAsync(string name);
    }
}
