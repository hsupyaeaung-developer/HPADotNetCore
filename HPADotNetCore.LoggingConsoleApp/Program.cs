
    // See https://aka.ms/new-console-template for more information
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

    Console.WriteLine("Hello, World!");

//get current project name
var projectName = Assembly.GetCallingAssembly().GetName().Name;

Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File($"logs/{projectName}.txt", rollingInterval: RollingInterval.Hour, fileSizeLimitBytes: 1024 * 100)
        .WriteTo
        .MSSqlServer(
        connectionString: "Server=DESKTOP-HDNAOSB\\SQL2022;Database=TestDb;User ID =sa;Password=sa@123;TrustServerCertificate=true;",
        sinkOptions: new MSSqlServerSinkOptions { TableName = "ConsoleLogEvents", AutoCreateSqlTable = true })
        .CreateLogger();

Log.Information("Hello, world!");
    
    int a = 10, b = 0;
    try
    {
        Log.Debug("Dividing {A} by {B}", a, b);
        Console.WriteLine(a / b);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Something went wrong");
    }
    finally
    {
        await Log.CloseAndFlushAsync();
    }
