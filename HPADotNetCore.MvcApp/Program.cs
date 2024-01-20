using HPADotNetCore.MvcApp;
using HPADotNetCore.MvcApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using Refit;
using RestSharp;
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
        sinkOptions: new MSSqlServerSinkOptions { TableName = "MVCLogEvents", AutoCreateSqlTable = true })
        .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    // Add services to the container.
    builder.Services.AddControllersWithViews(); 
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
},
ServiceLifetime.Transient,
ServiceLifetime.Transient);

#region Refit
builder.Services
    .AddRefitClient<IBlogApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(builder.Configuration.GetSection("RestApiUrl").Value!));

#endregion

#region HttpClient
builder.Services.AddScoped(x => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration.GetSection("RestApiUrl").Value!)
});
#endregion

#region RestClient
builder.Services.AddScoped(x => new RestClient(builder.Configuration.GetSection("RestApiUrl").Value!));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

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