using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Concurrency
{
    public interface IEmployee : IGrainWithGuidKey
    {
        Task Greeting(GreetingData data);
    }
}
