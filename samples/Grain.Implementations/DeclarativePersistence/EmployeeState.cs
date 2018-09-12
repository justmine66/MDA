using Grain.interfaces.DeclarativePersistence;

namespace Grain.Implementations.DeclarativePersistence
{
    public class EmployeeState
    {
        public int Level { get; set; }
        public IManager Manager { get; set; }
    }
}
