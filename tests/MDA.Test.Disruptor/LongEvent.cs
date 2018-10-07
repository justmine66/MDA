namespace MDA.Test.Disruptor
{
    public class LongEvent
    {
        public long Value { get; set; }

        public override string ToString()
        {
            return $"[Value: {Value}]";
        }
    }
}
