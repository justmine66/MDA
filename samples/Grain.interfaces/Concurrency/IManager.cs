using System.Threading.Tasks;
using Orleans;

namespace Grain.interfaces.Concurrency
{
    public interface IManager : IGrainWithGuidKey
    {
        Task AddDirectReport(IEmployee employee);
    }
}
