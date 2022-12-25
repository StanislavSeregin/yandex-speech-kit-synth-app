using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace App.Api;

public class CheckSettingsHostedService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public CheckSettingsHostedService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (File.Exists(Program.SettingsName))
        {
            return;
        }

        File.Create(Program.SettingsName);
    }
}
