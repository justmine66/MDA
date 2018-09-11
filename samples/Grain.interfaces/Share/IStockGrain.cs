using System.Threading.Tasks;

namespace Grain.interfaces.Share
{
    public interface IStockGrain : Orleans.IGrainWithStringKey
    {
        Task<string> GetPrice();
    }
}
