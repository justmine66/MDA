using System.Threading.Tasks;

namespace MDA.Messages
{
    public interface IMessageHandler<in TMessage> 
        where TMessage : IMessage
    {
        Task HandleAsync(TMessage message);
    }

    public interface IMessageHandler<in TMessage1, in TMessage2>
        where TMessage1 : IMessage
        where TMessage2 : IMessage
    {
        Task HandleAsync(TMessage1 message1, TMessage2 message2);
    }

    public interface IMessageHandler<in TMessage1, in TMessage2, in TMessage3>
        where TMessage1 : IMessage
        where TMessage2 : IMessage
        where TMessage3 : IMessage
    {
        Task HandleAsync(TMessage1 message1, TMessage2 message2, TMessage3 message3);
    }
}
