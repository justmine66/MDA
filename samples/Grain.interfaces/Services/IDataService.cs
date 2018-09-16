using Orleans.Services;
using System.Threading.Tasks;

namespace Grain.interfaces.Services
{
    public interface IDataService : IGrainService
    {
        Task MyMethod();
    }
}
