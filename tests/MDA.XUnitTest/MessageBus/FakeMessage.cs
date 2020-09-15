using MDA.MessageBus;

namespace MDA.XUnitTest.MessageBus
{
    public class FakeMessage : Message
    {
        public long Payload { get; set; }
    }
}
