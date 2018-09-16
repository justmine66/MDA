using Orleans.Services;

namespace Grain.interfaces.Services
{
    public interface IDataServiceClient : IGrainServiceClient<IDataService>, IDataService
    {
    }
}
