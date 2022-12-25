using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
            .Build()
            .RunAsync();
    }
}