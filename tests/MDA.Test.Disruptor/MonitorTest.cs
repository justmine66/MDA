using System;
using System.Threading;

namespace MDA.Test.Disruptor
{
    public class MonitorTest
    {
        static object lockObj = new object();
        static bool _go;

        internal static void Test()
        {
            new Thread(Work).Start(); //新线程会被阻塞，因为_go == false
            Console.ReadLine(); //等待用户输入

            lock (lockObj)
            {
                Console.WriteLine("另一个线程获取锁来改变阻塞条件");
                _go = true; //改变阻塞条件
                Monitor.Pulse(lockObj); //通知等待的队列。
                Console.WriteLine("已通知等待的队列。");
            }

            Console.WriteLine("主线程完毕。");
        }

        static void Work()
        {
            lock (lockObj)
            {
                Console.WriteLine("Work 获得锁");

                while (!_go) //只要_go字段是false，就等待。
                {
                    Monitor.Wait(lockObj); //在等待的时候，锁已经被释放了。
                }

                Console.WriteLine("Work 被唤醒了");
            }
        }
    }
}
