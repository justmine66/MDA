using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OrleansClient
{
    public class SpanDemo
    {
        public static void Test()
        {
            var arr = new byte[10];
            Span<byte> bytes = arr;
            Span<byte> sliceBytes = bytes.Slice(start: 5, length: 2);
            sliceBytes[0] = 42;
            sliceBytes[1] = 46;

            Console.WriteLine(sliceBytes[0]);
            Console.WriteLine(sliceBytes[1]);
            Console.WriteLine(arr[5]);
            Console.WriteLine(arr[6]);

            bytes[2] = 45;
            Console.WriteLine();
            Console.WriteLine(bytes[2]);
            Console.WriteLine(arr[2]);
        }

        public static void Test2()
        {
            Span<byte> bytes = stackalloc byte[2];
            bytes[0] = 42;
            bytes[1] = 43;
            Console.WriteLine(bytes[0]);
            Console.WriteLine(bytes[1]);
        }

        public static void Test3()
        {
            IntPtr ptr = Marshal.AllocHGlobal(1);
            try
            {
                Span<byte> bytes;
                unsafe
                {
                    bytes = new Span<byte>((byte*)ptr, 1);
                }
                bytes[0] = 42;
                Console.WriteLine(bytes[0]);
                Console.WriteLine(Marshal.ReadByte(ptr));
            }
            finally { Marshal.FreeHGlobal(ptr); }
        }

        struct MutableStruct { public int Value; }
        public static void Test4()
        {
            Span<MutableStruct> spanOfStructs = new MutableStruct[1];
            spanOfStructs[0].Value = 42;

            Console.WriteLine(spanOfStructs[0].Value);

            var listOfStructs = new List<MutableStruct> { new MutableStruct() };
            //listOfStructs[0].Value = 42;
        }

        public unsafe static int* Test5()
        {
            int i = 4;
            return &i;
        }
    }
}
