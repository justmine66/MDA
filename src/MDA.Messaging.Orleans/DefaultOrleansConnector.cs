using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using Polly;
using System;
using System.Threading;

namespace MDA.Messaging.Orleans
{
    public class DefaultOrleansConnector : IOrleansConnector
    {
        private readonly ILogger<DefaultOrleansConnector> _logger;
        private readonly int _retryCount;

        private int _connecting;
        private IClusterClient _client;

        public DefaultOrleansConnector(ILogger<DefaultOrleansConnector> logger, int retryCount = 5)
        {
            _logger = logger;
            _retryCount = retryCount;
        }

        public bool IsConnected => _client != null && _client.IsInitialized && _connecting == 1;

        public IClusterClient CreateConnect()
        {
            _logger.LogInformation("Orlean Client is creating.");

            var builder = new ClientBuilder()
              .UseLocalhostClustering()
              .Configure<ClusterOptions>(options =>
              {
                  options.ClusterId = "samples";
                  options.ServiceId = "OrleansSiloHost";
              })
              .ConfigureLogging(logging => logging.AddConsole());
            
            var policy = Policy.Handle<SiloUnavailableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            if (Interlocked.CompareExchange(ref _connecting, 0, 1) == 0)
            {
                policy.Execute(async () =>
                {
                    _client = builder.Build();
                    await _client.Connect();
                });

                if (IsConnected)
                {
                    _logger.LogInformation($"orleans client connected {_client.ToString()}.");
                }
                else
                {
                    _logger.LogCritical("FATAL ERROR: orleans client could not be connected.");
                }
            }

            return _client;
        }

        public void Dispose()
        {
            try
            {
                if (Interlocked.CompareExchange(ref _connecting, 1, 0) == 1)
                {
                    _client.Close();
                    _client.Abort();
                    _client.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.ToString());
            }
            finally
            {
                _connecting = 0;
                _client = null;
            }
        }
    }
}