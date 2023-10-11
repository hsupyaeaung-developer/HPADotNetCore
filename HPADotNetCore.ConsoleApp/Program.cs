using System;
using HPADotNetCore.ConsoleApp.AdoDotNetExamples;

namespace HPADotNetCore.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
            adoDotNetExample.Run();
        }
    }
}
