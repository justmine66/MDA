using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace PipelinesApp
{
    public class AnonymousPipe
    {
        public static AnonymousPipe New() => new AnonymousPipe();

        public void Server()
        {
            var process = new Process { StartInfo = { FileName = "PipelinesApp.dll" } };

            using (var pipe = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            {
                process.StartInfo.Arguments = pipe.GetClientHandleAsString();
                process.StartInfo.UseShellExecute = false;
                process.Start();

                pipe.DisposeLocalCopyOfClientHandle();

                using (var writer = new StreamWriter(pipe))
                {
                    writer.AutoFlush = true;
                    writer.WriteLine(Console.ReadLine());
                }
            }

            process.WaitForExit();
            process.Close();
        }

        public void Client(string[] args)
        {
            using (var reader = new StreamReader(new AnonymousPipeClientStream(PipeDirection.In, args[0])))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine("Echo: {0}", line);
                }
            }
        }
    }
}
