using HPADotNetCore.RestApi;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

//get current project name
var projectName = Assembly.GetCallingAssembly().GetName().Name;

Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File($"logs/{projectName}.txt", rollingInterval: RollingInterval.Hour, fileSizeLimitBytes: 1024 * 100)
        .WriteTo
        .MSSqlServer(
        connectionString: "Server=DESKTOP-HDNAOSB\\SQL2022;Database=TestDb;User ID =sa;Password=sa@123;TrustServerCertificate=true;",
        sinkOptions: new MSSqlServerSinkOptions { TableName = "RestApiLogEvents", AutoCreateSqlTable = true })
        .CreateLogger();

    try
    {

    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<AppDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
    },
    ServiceLifetime.Transient,
    ServiceLifetime.Transient);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Application terminated unexpectedly");
    }
    finally
    {
        Log.CloseAndFlush();
    }