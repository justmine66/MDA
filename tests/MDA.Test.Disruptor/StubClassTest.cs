using System;
using System.Diagnostics;

namespace MDA.Test.Disruptor
{
    public static class StubClassTest
    {
        public static void Test()
        {
            // 调用的目标实例。
            var instance = new StubClass();

            // 使用反射找到的方法。
            var method = typeof(StubClass).GetMethod(nameof(StubClass.Test), new[] {typeof(int)});

            // 将反射找到的方法创建一个委托。
            var func = InstanceMethodBuilder<int, int>.CreateInstanceMethod(instance, method);

            // 跟被测方法功能一样的纯委托。
            Func<int, int> pureFunc = value => value;

            // 测试次数。
            var count = 10000000;

            // 直接调用。
            var watch = new Stopwatch();
            watch.Start();
            for (var i = 0; i < count; i++)
            {
                var result = instance.Test(5);
            }

            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} - {count} 次 - 直接调用");

            // 使用同样功能的 Func 调用。
            watch.Restart();
            for (var i = 0; i < count; i++)
            {
                var result = pureFunc(5);
            }

            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} - {count} 次 - 使用同样功能的 Func 调用");

            // 使用反射创建出来的委托调用。
            watch.Restart();
            for (var i = 0; i < count; i++)
            {
                var result = func(5);
            }

            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} - {count} 次 - 使用反射创建出来的委托调用");

            // 使用反射得到的方法缓存调用。
            watch.Restart();
            for (var i = 0; i < count; i++)
            {
                var result = method.Invoke(instance, new object[] { 5 });
            }

            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} - {count} 次 - 使用反射得到的方法缓存调用");

            // 直接使用反射调用。
            watch.Restart();
            for (var i = 0; i < count; i++)
            {
                var result = typeof(StubClass).GetMethod(nameof(StubClass.Test), new[] { typeof(int) })
                    ?.Invoke(instance, new object[] { 5 });
            }

            watch.Stop();
            Console.WriteLine($"{watch.Elapsed} - {count} 次 - 直接使用反射调用");
        }
    }

    public class StubClass
    {
        public int Test(int i)
        {
            return i;
        }
    }
}
