using HPADotNetCore.ConsoleApp.AdoDotNetExamples;
using HPADotNetCore.ConsoleApp.DapperExamples;
using HPADotNetCore.ConsoleApp.EFCoreExamples;
using System;


namespace HPADotNetCore.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // DapperExample dapperExamples = new DapperExample();
            //dapperExamples.Run();

            EFCoreExample eFCoreExamples = new EFCoreExample();
            eFCoreExamples.Run();
        }
    }
}
