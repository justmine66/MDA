using System.Threading.Tasks;

namespace EBank.Domain.Repositories.MySql
{
    public interface IBankAccountRepository
    {
        Task<bool> HadAccountNameAsync(string name);
    }
}
