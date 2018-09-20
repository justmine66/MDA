using Grain.interfaces.Stream;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Stream
{
    public class RandomSender : Orleans.Grain, IRandomSender
    {
        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider("SMSProvider");
            var stream = streamProvider.GetStream<int>(Guid.NewGuid(), "RANDOMDATA");

            RegisterTimer(s => stream.OnNextAsync(new System.Random().Next()),
                null,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1));

            return base.OnActivateAsync();
        }
    }
}
