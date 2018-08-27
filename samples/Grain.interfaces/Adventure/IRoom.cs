using Orleans;
using System.Threading.Tasks;

namespace Grain.interfaces.Adventure
{
    public interface IRoom : IGrainWithIntegerKey
    {
        Task<string> Description(PlayerInfo whoisAsking);
        Task SetInfo(RoomInfo info);
        Task<IRoom> ExitTo(string direction);

        Task Enter(PlayerInfo player);
        Task Exit(PlayerInfo player);

        Task Enter(MonsterInfo monster);
        Task Exit(MonsterInfo monster);

        Task Drop(Thing thing);
        Task Take(Thing thing);
        Task<Thing> FindThing(string name);

        Task<PlayerInfo> FindPlayer(string name);
        Task<MonsterInfo> FindMonster(string name);
    }
}
