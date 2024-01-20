    using HPADotNetCore.MinimalApi;
    using HPADotNetCore.MinimalApi.Features.Blog;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json.Serialization;
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
        sinkOptions: new MSSqlServerSinkOptions { TableName = "MinimalApiLogEvents", AutoCreateSqlTable = true })
        .CreateLogger();
try
    {
        Log.Information("Starting web application");

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();
        //builder.Services.AddControllersWithViews().AddJsonOptions(opt =>
        //{
        //    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
        //    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
        //});

        builder.Services.ConfigureHttpJsonOptions(option =>
    {
        option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        option.SerializerOptions.PropertyNamingPolicy = null;
    });

    // Add services to the container.
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

    app.AddBlogService();

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
