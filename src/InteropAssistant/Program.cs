using System;
using System.IO;

namespace InteropAssistant
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = HeaderProcessor.ProcessInput(File.ReadAllText(@"C:\wayk\dev\WaykNow\include\Wayk\NowPrimitives.h"));
        }
    }
}
