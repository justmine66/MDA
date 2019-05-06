namespace MDA.Messaging
{
    public abstract class SequenceMessage : Message, ISequenceMessage
    {
        public long Sequence { get; set; }
    }
}
