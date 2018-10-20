using MDA.Disruptor;

namespace MDA.Tests.Disruptor.Support
{
    public class StubEvent
    {
        public static IEventTranslatorTwoArg<StubEvent, int, string> Translator = new EventTranslatorTwoArg();
        public static IEventFactory<StubEvent> EventFactory = new EventFactory();

        public int Value { get; set; }
        public string TestString { get; set; }

        public StubEvent(int i)
        {
            Value = i;
        }

        public override int GetHashCode()
        {
            var prime = 31;
            int result = 1;
            result = (prime * result) + Value;
            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(obj, this)) return true;

            return obj is StubEvent other && other.Value == Value;
        }
    }

    public class EventTranslatorTwoArg : IEventTranslatorTwoArg<StubEvent, int, string>
    {
        public void TranslateTo(StubEvent @event, long sequence, int arg0, string arg1)
        {
            @event.Value = arg0;
            @event.TestString = arg1;
        }
    }

    public class EventFactory : IEventFactory<StubEvent>
    {
        public StubEvent NewInstance()
        {
            return new StubEvent(-1);
        }
    }
}
