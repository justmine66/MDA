namespace MDA.Shared.Serialization
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T obj);

        T DeSerialize<T>(byte[] bytes);
    }
}
