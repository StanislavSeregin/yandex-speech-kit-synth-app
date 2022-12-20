using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace YandexSpeechKitSynthClient.Api;

internal class Program
{
    public static Task Main(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseStartup<Startup>())
            .Build()
            .RunAsync();
    }
}