using System;
using System.Threading.Tasks;
using Grain.interfaces.Services;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Core;
using Orleans.Runtime;

namespace Grain.Implementations.Services
{
    public class LightstreamerDataService : GrainService, IDataService
    {
        readonly IGrainFactory GrainFactory;
        public LightstreamerDataService(
            IServiceProvider services,
            IGrainIdentity id,
            Silo silo,
            ILoggerFactory loggerFactory,
            IGrainFactory grainFactory)
            : base(id, silo, loggerFactory)
        {
            GrainFactory = grainFactory;
        }

        public override Task Init(IServiceProvider serviceProvider)
        {
            return base.Init(serviceProvider);
        }

        public override async Task Start()
        {
            await base.Start();
        }

        public override Task Stop()
        {
            return base.Stop();
        }

        public Task MyMethod()
        {
            throw new System.NotImplementedException();
        }
    }
}
