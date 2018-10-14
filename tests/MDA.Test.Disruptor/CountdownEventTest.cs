using System;
using System.Threading;

namespace MDA.Test.Disruptor
{
    public class CountdownEventTest
    {
        private static CountdownEvent _countdown = new CountdownEvent(2);

        private static void Run(string message, int seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ": " + message);
            _countdown.Signal();
        }

        public static void Test()
        {
            Console.WriteLine("开始两个操作");
            Run("操作1完成", 1);
            Run("操作2完成", 2);
            _countdown.Wait();
            Console.WriteLine("两个操作都已完成.");
        }

        public static void Test1()
        {
            Console.WriteLine("开始两个操作");
            var t1 = new Thread(() => Run("操作1完成", 1));
            var t2 = new Thread(() => Run("操作2完成", 1));
            t1.Start();
            t2.Start();
            _countdown.Wait();
            Console.WriteLine("两个操作都已完成.");
        }
    }
}
