using System;
using Grain.interfaces.Observers;

namespace OrleansClient.Observers
{
    public class Chat : IChat
    {
        public void ReceiveMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
