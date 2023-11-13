using HPADotNetCore.ConsoleApp.AdoDotNetExamples;
using HPADotNetCore.ConsoleApp.DapperExamples;
using HPADotNetCore.ConsoleApp.EFCoreExamples;
using HPADotNetCore.ConsoleApp.HttpClientExamples;
using HPADotNetCore.ConsoleApp.RefitExamples;
using HPADotNetCore.ConsoleApp.RestClientExamples;
using System;
using System.Threading.Tasks;

namespace HPADotNetCore.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // server name
            // database name
            // user name
            // password

            // Ctrl + ., enter => suggestion 
            // F9 => Breakpoint
            // Ctrl + R, R => Rename
            // F10 => summary trace
            // F11 => detail trace
            // FiraCode

            // Ctrl + M, O // Collapse Method
            // Ctrl + M, A // Collapse Region

            // Ctrl + K, D

            // Ctrl + K, C // disable comment
            // Ctrl + K, U // enable comment

            // UI BL DA => Db
            // User Interface
            // Business Logic
            // Data Access
            // Database // CRUD -- Read, Create, Edit, Update, Delete

            //AdoDotNetExample adoDotNetExample = new AdoDotNetExample();
            //adoDotNetExample.Run();

            //DapperExample dapperExample = new DapperExample();
            //dapperExample.Run();

            //EFCoreExample eFCoreExample = new EFCoreExample();
            //eFCoreExample.Run();


            Console.WriteLine("waiting for api...");
            Console.ReadKey();
            //HttpClientExample  httpClientExample = new HttpClientExample();

            //await httpClientExample.Run();//HttpClientExample  httplClientExample = new HttpClientExample();
            //RestClientExample restClientExample = new RestClientExample();
            //await restClientExample.Run();

            RefitExample refitExample = new RefitExample();
            await refitExample.Run();

        }
    }
}
