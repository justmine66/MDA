using Orleans;

namespace Grain.interfaces.Observers
{
    public interface IChat : IGrainObserver
    {
        void ReceiveMessage(string message);
    }
}
