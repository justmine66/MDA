using System.Threading.Tasks;

namespace Grain.interfaces.ActorIdentity
{
    public interface IExampleGrain : Orleans.IGrainWithIntegerCompoundKey
    {
        Task Hello();
    }
}
