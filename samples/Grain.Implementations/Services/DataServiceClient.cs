using Grain.interfaces.Services;
using Orleans.Runtime.Services;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Services
{
    public class DataServiceClient : GrainServiceClient<IDataService>, IDataServiceClient
    {

        public DataServiceClient(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        public Task MyMethod() => GrainService.MyMethod();

        Task IDataService.MyMethod()
        {
            throw new NotImplementedException();
        }
    }
}
