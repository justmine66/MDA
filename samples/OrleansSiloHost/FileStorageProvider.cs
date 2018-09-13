using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.Threading.Tasks;

namespace OrleansSiloHost
{
    public class FileStorageProvider : IStorageProvider
    {
        private JsonSerializerSettings _jsonSettings;
        public ILogger Logger { get; set; }
        public string RootDirectory { get; set; }

        public string Name { get; set; }

        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            throw new NotImplementedException();
        }

        public Task Close()
        {
            throw new NotImplementedException();
        }

        public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
        {
            Name = name;

            return Task.CompletedTask;
        }

        public async Task ReadStateAsync(string grainType, GrainReference grainRef, IGrainState grainState)
        {
            var collectionName = grainState.GetType().Name;
            var key = grainRef.ToKeyString();
            var fName = key + "." + collectionName;
            var path = System.IO.Path.Combine(RootDirectory, fName);

            var fileInfo = new System.IO.FileInfo(path);
            if (!fileInfo.Exists)
                return;

            using (var stream = fileInfo.OpenText())
            {
                var storedData = await stream.ReadToEndAsync();

                grainState.State = JsonConvert.DeserializeObject(storedData, grainState.State.GetType(), _jsonSettings);
            }
        }

        public async Task WriteStateAsync(string grainType, GrainReference grainRef, IGrainState grainState)
        {
            var storedData = JsonConvert.SerializeObject(grainState.State, _jsonSettings);

            var collectionName = grainState.GetType().Name;
            var key = grainRef.ToKeyString();

            var fName = key + "." + collectionName;
            var path = System.IO.Path.Combine(RootDirectory, fName);

            var fileInfo = new System.IO.FileInfo(path);

            using (var stream = new System.IO.StreamWriter(
                       fileInfo.Open(System.IO.FileMode.Create,
                                     System.IO.FileAccess.Write)))
            {
                await stream.WriteAsync(storedData);
            }
        }
    }
}
