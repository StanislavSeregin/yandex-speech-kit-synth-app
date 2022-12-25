using App.YandexClient;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api.HostedServices;

public class InitAppSettingsHostedService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (File.Exists(Program.SETTINGS_NAME))
        {
            return;
        }

        var jObject = new JsonObject
        {
            [Startup.YANDEX_CLIENT_CONFIGURATION_KEY] = JsonSerializer.SerializeToNode(new YandexClientConfiguration())
        };

        await using var file = File.Create(Program.SETTINGS_NAME);
        await using var writer = new Utf8JsonWriter(file, new JsonWriterOptions { Indented = true });
        jObject.WriteTo(writer);
    }
}
