using System.Threading.Tasks;
using HelloWorld.Interfaces;
using Orleans;

namespace HelloWorld.Grains
{
    public class Hello : Grain, IHello
    {
        public Task<string> SayHello(string greeting)
        {
            return Task.FromResult("You said: '" + greeting + "', I say: Hello!");
        }
    }
}
