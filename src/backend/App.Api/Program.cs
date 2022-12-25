using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace App.Api;

internal class Program
{
    public const string SettingsName = "appsettings.json";

    public static Task Main(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder => builder
                .AddJsonFile(
                    SettingsName,
                    optional: true,
                    reloadOnChange: true))
            .ConfigureWebHostDefaults(builder => builder
                .UseStartup<Startup>())
            .Build()
            .RunAsync();
    }
}