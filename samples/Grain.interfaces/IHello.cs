using System.Threading.Tasks;

namespace Grain.interfaces
{
    public interface IHello : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHelloAsync(string greeting);
    }
}
