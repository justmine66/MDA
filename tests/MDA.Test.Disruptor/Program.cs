using System;

namespace MDA.Test.Disruptor
{
    class Program
    {
        static void Main(string[] args)
        {
            //new FalseSharingTest().StartTest();
            StructProxyTest.Test();

            Console.Read();
        }
    }
}
