namespace MDA.Persistence
{
    public interface IAppStateProvider
    {
        T Get<T>(string principal) where T : class;
    }
}
