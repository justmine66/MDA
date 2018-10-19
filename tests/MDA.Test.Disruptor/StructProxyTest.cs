using MDA.Disruptor;
using MDA.Disruptor.Infrastracture;

namespace MDA.Test.Disruptor
{
    public class StructProxyTest
    {
        public static void Test()
        {
            IEventHandler<LongEvent> target = new LongEventHandler();
            var eventHandlerProxy = StructProxy.CreateProxyInstance(target);
            var eventHandlerProxy2 = StructProxy.CreateProxyInstance(target);
            eventHandlerProxy.OnEvent(new LongEvent() {Value = 123}, 12, true);
            eventHandlerProxy2.OnEvent(new LongEvent() {Value = 123}, 12, true);
        }
    }
}
