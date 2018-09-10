using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.ActorCollection
{
    public interface IEmployee : IGrainWithGuidKey
    {
        Task<int> GetLevel();
        Task Promote(int newLevel);

        Task<IManager> GetManager();
        Task SetManager(IManager manager);

        Task Greeting(IEmployee from, string message);
    }
}
