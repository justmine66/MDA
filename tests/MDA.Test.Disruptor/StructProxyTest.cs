using MDA.Disruptor;
using MDA.Disruptor.Internal;

namespace MDA.Test.Disruptor
{
    public class StructProxyTest
    {
        public static void Test()
        {
            IEventHandler<LongEvent> target = new LongEventHandler();
            var eventHandlerProxy = StructProxy.CreateProxyInstance(target);
            eventHandlerProxy.OnEvent(new LongEvent() {Value = 123}, 12, true);
        }
    }
}
