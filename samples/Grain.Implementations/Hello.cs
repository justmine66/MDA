using Grain.interfaces;
using System.Threading.Tasks;

namespace Grain.Implementations
{
    public class Hello : Orleans.Grain, IHello
    {
        public Task<string> SayHelloAsync(string greeting)
        {
            return Task.FromResult($"You said: '{greeting}', I say: Hello!");
        }
    }
}
