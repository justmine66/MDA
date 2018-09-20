using System;
using System.Threading.Tasks;
using Grain.interfaces.Stream;
using Orleans;
using Orleans.Streams;

namespace Grain.Implementations.Stream
{
    [ImplicitStreamSubscription("RANDOMDATA")]
    public class RandomReceiver : Orleans.Grain, IRandomReceiver
    {
        public async Task GetMsg()
        {
            var guid = this.GetPrimaryKey();
            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(guid, "RANDOMDATA");
            await stream.SubscribeAsync<int>(async (data, token) => Console.WriteLine(data));
        }
    }
}
