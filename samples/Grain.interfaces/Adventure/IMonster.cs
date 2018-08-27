using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grain.interfaces.Adventure
{
    public interface IMonster : IGrainWithIntegerKey
    {
        Task<string> Name();
        Task SetInfo(MonsterInfo info);

        Task SetRoom(IRoom room);
        Task<IRoom> Room();

        Task<string> Kill(IRoom room);
    }
}
