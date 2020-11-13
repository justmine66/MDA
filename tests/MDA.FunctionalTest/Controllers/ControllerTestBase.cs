using System;
using System.Net.Http;

namespace MDA.FunctionalTest.Controllers
{
    public abstract class ControllerTestBase
    {
        protected HttpClient Client { get; }

        protected IServiceProvider Services { get; }

        protected ControllerTestBase(EBandApiTestFixture fixture)
        {
            Client = fixture.Client;
            Services = fixture.Services;
        }
    }
}
