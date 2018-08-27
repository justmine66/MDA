using Grain.interfaces.Adventure;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grain.Implementations.Adventure
{
    public class Player : Orleans.Grain, IPlayer
    {
        private IRoom _room;
        private List<Thing> _things = new List<Thing>();

        bool _killed = false;

        PlayerInfo _myInfo;

        public override Task OnActivateAsync()
        {
            _myInfo = new PlayerInfo { Key = this.GetPrimaryKey(), Name = "nobody" };
            return base.OnActivateAsync();
        }

        public async Task Die()
        {
            var tasks = new List<Task<string>>();
            foreach (var thing in new List<Thing>(_things))
            {
                tasks.Add(this.Drop(thing));
            }
            await Task.WhenAll(tasks);

            // Exit the game
            if (_room != null)
            {
                await _room.Exit(_myInfo);
                _room = null;
                _killed = true;
            }
        }

        public Task<string> Name()
        {
            return Task.FromResult(_myInfo.Name);
        }

        public Task<string> Play(string command)
        {
            throw new NotImplementedException();
        }

        public Task<IRoom> Room()
        {
            return Task.FromResult(_room);
        }

        public Task SetName(string name)
        {
            throw new NotImplementedException();
        }

        public Task SetRoom(IRoom room)
        {
            throw new NotImplementedException();
        }

        private async Task<string> Drop(Thing thing)
        {
            if (_killed)
                return await CheckAlive();

            if (thing != null)
            {
                _things.Remove(thing);
                await _room.Drop(thing);
                return "Okay.";
            }
            else
                return "I don't understand.";
        }

        private async Task<string> Take(Thing thing)
        {
            if (_killed)
                return await CheckAlive();

            if (thing != null)
            {
                _things.Add(thing);
                await _room.Take(thing);
                return "Okay.";
            }
            else
                return "I don't understand.";
        }

        private async Task<string> CheckAlive()
        {
            if (!_killed)
                return null;

            var room = GrainFactory.GetGrain<IRoom>(-2);
            return await room.Description(_myInfo);
        }

        async Task<string> Kill(string target)
        {
            if (_things.Count == 0)
                return "With what? Your bare hands?";

            var player = await _room.FindPlayer(target);
            if (player != null)
            {
                var weapon = _things.Where(t => t.Category == "weapon").FirstOrDefault();
                if (weapon != null)
                {
                    await GrainFactory.GetGrain<IPlayer>(player.Key).Die();
                    return target + " is now dead.";
                }
                return "With what? Your bare hands?";
            }

            var monster = await _room.FindMonster(target);
            if (monster != null)
            {
                var weapons = monster.KilledBy.Join(_things, id => id, t => t.Id, (id, t) => t);
                if (weapons.Count() > 0)
                {
                    await GrainFactory.GetGrain<IMonster>(monster.Id).Kill(_room);
                    return target + " is now dead.";
                }
                return "With what? Your bare hands?";
            }
            return "I can't see " + target + " here. Are you sure?";
        }
    }
}
