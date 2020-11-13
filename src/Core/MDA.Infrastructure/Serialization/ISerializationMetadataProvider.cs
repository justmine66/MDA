namespace MDA.Infrastructure.Serialization
{
    public interface ISerializationMetadataProvider
    {
        string[] IgnoreKeys { get; }
    }
}
