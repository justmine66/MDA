namespace MDA.StateBackend.RDBMS.Shared
{
    public interface IRelationalDbStorageFactory
    {
        IRelationalDbStorage CreateRelationalDbStorage();
    }
}
