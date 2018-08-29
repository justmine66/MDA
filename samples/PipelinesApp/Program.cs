using System;

namespace PipelinesApp
{
    class Program
    {
        static void Main(string[] args)
        {
            AnonymousPipe.New().Server();

            Console.Read();
        }
    }
}
