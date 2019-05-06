namespace MDA.Messaging
{
    public interface ISequenceMessage : IMessage
    {
        long Sequence { get; set; }
    }
}
