using Grain.interfaces.Adventure;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Grain.Implementations.Adventure
{
    public class Monster : Orleans.Grain, IMonster
    {
        private MonsterInfo _monsterInfo = new MonsterInfo();
        private IRoom _room;

        public override Task OnActivateAsync()
        {
            _monsterInfo.Id = this.GetPrimaryKeyLong();

            RegisterTimer((_) => Move(), null, TimeSpan.FromSeconds(150), TimeSpan.FromMinutes(150));

            return base.OnActivateAsync();
        }

        public Task<string> Name()
        {
            return Task.FromResult(_monsterInfo.Name);
        }

        public Task SetInfo(MonsterInfo info)
        {
            _monsterInfo = info;
            return Task.CompletedTask;
        }

        private async Task Move()
        {
            var directions = new string[] { "north", "south", "west", "east" };

            var rand = new Random().Next(0, 4);
            IRoom nextRoom = await _room.ExitTo(directions[rand]);

            if (null == nextRoom)
                return;

            await _room.Exit(_monsterInfo);
            await nextRoom.Enter(_monsterInfo);

            _room = nextRoom;
        }

        public async Task SetRoom(IRoom room)
        {
            if (_room != null)
                await _room.Exit(_monsterInfo);
            _room = room;
            await _room.Enter(_monsterInfo);
        }

        public Task<IRoom> Room()
        {
            return Task.FromResult(_room);
        }

        public Task<string> Kill(IRoom room)
        {
            if (_room != null)
            {
                if (_room.GetPrimaryKey() != room.GetPrimaryKey())
                {
                    return Task.FromResult(_monsterInfo.Name + " snuck away. You were too slow!");
                }

                return _room.Exit(_monsterInfo).ContinueWith(t => _monsterInfo.Name + " is dead.");
            }
            return Task.FromResult(_monsterInfo.Name + " is already dead. You were too slow and someone else got to him!");
        }
    }
}
