using System;
using System.IO;
using System.Text;

namespace PipelinesApp
{
    public class StreamDemo
    {
        public void Test()
        {
            using (var stream = new MemoryStream())
            {
                WriteData(stream);
                stream.Position = 0;
                ReadData(stream);
            }
        }

        private void WriteData(Stream stream)
        {
            var buffer = Encoding.ASCII.GetBytes("hello, world!");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        private void ReadData(Stream stream)
        {
            var buffer = new byte[256];
            int readedUnitLength = 0;
            do
            {
                readedUnitLength = stream.Read(buffer, 0, buffer.Length);
                if (readedUnitLength > 0)
                {
                    string s = Encoding.ASCII.GetString(buffer, 0, readedUnitLength);
                    Console.Write(s);
                }
            } while (readedUnitLength > 0);
        }
    }
}
