using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grain.interfaces.Adventure
{
    public interface IPlayer : IGrainWithGuidKey
    {
        Task<string> Name();
        Task SetName(string name);

        Task SetRoom(IRoom room);
        Task<IRoom> Room();

        Task Die();

        Task<string> Play(string command);
    }
}
