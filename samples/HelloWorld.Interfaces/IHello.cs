using Orleans;
using System.Threading.Tasks;

namespace HelloWorld.Interfaces
{
    public interface IHello: IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
    }
}
