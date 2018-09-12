using Grain.interfaces.DeclarativePersistence;
using System.Collections.Generic;

namespace Grain.Implementations.DeclarativePersistence
{
    public class ManagerState
    {
        public List<IEmployee> Reports { get; set; }
    }
}
