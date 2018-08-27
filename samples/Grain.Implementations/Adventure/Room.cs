using Grain.interfaces.Adventure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grain.Implementations.Adventure
{
    internal class Room : Orleans.Grain, IRoom
    {
        private string _description;
        private List<PlayerInfo> _players = new List<PlayerInfo>();
        private List<MonsterInfo> _monsters = new List<MonsterInfo>();
        private List<Thing> _things = new List<Thing>();

        private Dictionary<string, IRoom> _exits = new Dictionary<string, IRoom>();

        public Task<string> Description(PlayerInfo whoisAsking)
        {
            throw new NotImplementedException();
        }

        public Task Drop(Thing thing)
        {
            _things.RemoveAll(x => x.Id == thing.Id);
            _things.Add(thing);
            return Task.CompletedTask;
        }

        public Task Enter(PlayerInfo player)
        {
            _players.RemoveAll(p => p.Key == player.Key);
            _players.Add(player);

            return Task.CompletedTask;
        }

        public Task Enter(MonsterInfo monster)
        {
            _monsters.RemoveAll(x => x.Id == monster.Id);
            _monsters.Add(monster);
            return Task.CompletedTask;
        }

        public Task Exit(PlayerInfo player)
        {
            _players.RemoveAll(x => x.Key == player.Key);
            return Task.CompletedTask;
        }

        public Task Exit(MonsterInfo monster)
        {
            _monsters.RemoveAll(x => x.Id == monster.Id);
            return Task.CompletedTask;
        }

        public Task<IRoom> ExitTo(string direction)
        {
            return Task.FromResult((_exits.ContainsKey(direction)) ? _exits[direction] : null);
        }

        public Task<MonsterInfo> FindMonster(string name)
        {
            name = name.ToLower();
            return Task.FromResult(_monsters.Where(x => x.Name.ToLower().Contains(name)).FirstOrDefault());
        }

        public Task<PlayerInfo> FindPlayer(string name)
        {
            name = name.ToLower();
            return Task.FromResult(_players.Where(x => x.Name.ToLower().Contains(name)).FirstOrDefault());
        }

        public Task<Thing> FindThing(string name)
        {
            return Task.FromResult(_things.Where(x => x.Name == name).FirstOrDefault());
        }

        public Task SetInfo(RoomInfo info)
        {
            _description = info.Description;
            foreach (var kv in info.Directions)
            {
                _exits[kv.Key] = GrainFactory.GetGrain<IRoom>(kv.Value);
            }
            return Task.CompletedTask;
        }

        public Task Take(Thing thing)
        {
            _things.RemoveAll(x => x.Name == thing.Name);
            return Task.CompletedTask;
        }
    }
}
