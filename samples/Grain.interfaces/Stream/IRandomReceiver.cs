using System.Threading.Tasks;

namespace Grain.interfaces.Stream
{
    public interface IRandomReceiver
    {
        Task GetMsg();
    }
}
