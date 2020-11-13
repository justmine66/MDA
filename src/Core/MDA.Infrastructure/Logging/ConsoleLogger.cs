using System;

namespace MDA.Infrastructure.Logging
{
    public class ConsoleLogger
    {
        public static void WriteLine(
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor,
            string messageFormat,
            params object[] args)
        {
            var bgColor = Console.BackgroundColor;
            var foreColor = Console.ForegroundColor;

            Console.BackgroundColor = backgroundColor;
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(messageFormat, args);

            Console.BackgroundColor = bgColor;
            Console.ForegroundColor = foreColor;
        }

        public static void LogDebug(string name, string format, params object[] args)
        {
            WriteLine(ConsoleColor.White, ConsoleColor.Black, "Debug => " + format, args);
        }

        public static void LogInfo(string name, string format, params object[] args)
        {
            WriteLine(ConsoleColor.Green, ConsoleColor.Black, "Info => " + format, args);
        }

        public static void LogWarn(string name, string format, params object[] args)
        {
            WriteLine(ConsoleColor.Yellow, ConsoleColor.Black, "Warn => " + format, args);
        }

        public static void LogError(string name, string format, params object[] args)
        {
            WriteLine(ConsoleColor.Red, ConsoleColor.Black, "Error => " + format, args);
        }

        public static void LogFatal(string name, string format, params object[] args)
        {
            WriteLine(ConsoleColor.DarkRed, ConsoleColor.White, "Fatal => " + format, args);
        }
    }
}
