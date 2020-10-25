using System.Threading.Tasks;

namespace EBank.ApiServer.Application.Business
{
    public interface IBankAccountRepository
    {
        Task<bool> HadAccountNameAsync(string name);
    }
}
