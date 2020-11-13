using System;
using System.Net.Http;

namespace MDA.FunctionalTest
{
    public class EBandApiTestFixture
    {
        public IServiceProvider Services { get; }

        public HttpClient Client { get; }

        public EBandApiTestFixture(IServiceProvider serviceProvider, HttpClient httpClient)
        {
            Services = serviceProvider;

            Client = httpClient;

            Client.BaseAddress = new Uri(EBankApiServer.Origin);
        }
    }
}
