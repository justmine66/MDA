using System;
using System.Globalization;
using System.Threading;

namespace MDA.Test.Disruptor
{
    public class YieldSleep0Sleep1Test
    {
        public static void Test()
        {
            //1. 单核测试才有意义
            //Yield();
            //Sleep0();
            //Sleep1();

            //2. 多核：线程数大于CPU核数才有意义。
            //MultiThreadingWithSamePriority();
            //MultiThreadingWithDifferentPriority();
        }

        public static void Yield()
        {
            while (true)
            {
                WasteTime(); //模拟执行某个操作
                Thread.Yield();
            }
        }

        public static void Sleep0()
        {
            while (true)
            {
                WasteTime(); //模拟执行某个操作
                Thread.Yield();
            }
        }

        public static void Sleep1()
        {
            string s = "";
            while (true)
            {
                WasteTime(); //模拟执行某个操作
                Thread.Yield();
            }
        }

        public static void MultiThreadingWithSamePriority()
        {
            var number = Environment.ProcessorCount * 2;
            var theads = new Thread[number];
            for (int i = 0; i < number; i++)
            {
                theads[i] = new Thread((state) =>
                     {
                         while (true)
                         {
                             WasteTime();
                             Console.WriteLine($"Thread {state} ==========");
                             if (int.TryParse(state.ToString(), out int stateInt) && stateInt < 1)
                             {
                                 //Thread.Yield();
                                 //Thread.Sleep(0);
                                 //Thread.Sleep(1);
                             }
                         }
                     })
                { IsBackground = true };
            }

            for (int i = 0; i < number; i++)
            {
                theads[i].Start(i);
            }

            Console.ReadKey();

            for (int i = 0; i < number; i++)
            {
                theads[i].Abort();
            }

            Console.ReadKey();
        }

        public static void MultiThreadingWithDifferentPriority()
        {
            var number = Environment.ProcessorCount * 2;
            var theads = new Thread[number];
            for (int i = 0; i < number; i++)
            {
                theads[i] = new Thread((state) =>
                    {
                        while (true)
                        {
                            WasteTime();
                            Console.WriteLine($"Thread {state} ==========");
                            if (int.TryParse(state.ToString(), out int stateInt) && stateInt < 1)
                            {
                                //Thread.Yield();
                                //Thread.Sleep(0);
                                //Thread.Sleep(1);
                            }
                        }
                    })
                { IsBackground = true };

                if (i > 4)
                {
                    theads[i].Priority = ThreadPriority.AboveNormal;
                }
            }

            for (int i = 0; i < number; i++)
            {
                theads[i].Start(i);
            }

            Console.ReadKey();

            for (int i = 0; i < number; i++)
            {
                theads[i].Abort();
            }

            Console.ReadKey();
        }

        private static void WasteTime()
        {
            // 耗时约 200ms
            DateTime dt = DateTime.Now;
            string s = "";
            while (DateTime.Now.Subtract(dt).Milliseconds <= 200)
            {
                s = DateTime.Now.ToString(CultureInfo.InvariantCulture); //加上这句,防止被编译器优化
            }
        }
    }
}
