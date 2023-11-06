using HPADotNetCore.ConsoleApp.AdoDotNetExamples;
using HPADotNetCore.ConsoleApp.DapperExamples;
using HPADotNetCore.ConsoleApp.EFCoreExamples;
using HPADotNetCore.ConsoleApp.HttpClientExamples;
using System;
using System.Threading.Tasks;

namespace HPADotNetCore.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // DapperExample dapperExamples = new DapperExample();
            //dapperExamples.Run();
            Console.WriteLine("waiting for api...");
            Console.ReadKey();
            HttpClientExample  httplClientExample = new HttpClientExample();
            await httplClientExample.Run();

            
        }
    }
}
