using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using System.Threading.Tasks;

namespace App.Api;

internal class Program
{
    public const string SETTINGS_NAME = "appsettings.json";

    public static Task Main(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder => builder
                .AddJsonFile(
                    SETTINGS_NAME,
                    optional: true,
                    reloadOnChange: true))
            .ConfigureWebHostDefaults(builder => builder
                .UseStartup<Startup>())
#if !DEBUG
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .WriteTo.File("./logs.txt"))
#endif
            .Build()
            .RunAsync();
    }
}