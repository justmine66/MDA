namespace MDA.Messages.Partitioners
{
    public interface IMessagePartitioner
    {
        void Setup(int numberOfChannels);

        int SelectChannel(IMessage record);
    }

    public class MessagePartitioner : IMessagePartitioner
    {
        public void Setup(int numberOfChannels)
        {
            throw new System.NotImplementedException();
        }

        public int SelectChannel(IMessage record)
        {
            throw new System.NotImplementedException();
        }

        public bool IsBroadcast { get; }
    }
}
