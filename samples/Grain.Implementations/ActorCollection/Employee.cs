using Grain.interfaces.ActorCollection;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.ActorCollection
{
    public class Employee : Orleans.Grain, IEmployee
    {
        private int _level;
        private IManager _manager;

        public Task<int> GetLevel()
        {
            return Task.FromResult(_level);
        }

        public Task<IManager> GetManager()
        {
            throw new NotImplementedException();
        }

        public Task Greeting(IEmployee from, string message)
        {
            Console.WriteLine("{0} said: {1}", from.GetPrimaryKey().ToString(), message);

            return Task.CompletedTask;
        }

        public Task Promote(int newLevel)
        {
            _level = newLevel;
            return Task.CompletedTask;
        }

        public Task SetManager(IManager manager)
        {
            _manager = manager;
            return Task.CompletedTask;
        }
    }
}
