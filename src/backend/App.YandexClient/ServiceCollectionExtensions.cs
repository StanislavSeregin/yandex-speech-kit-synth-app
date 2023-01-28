using ITCC.YandexSpeechKitClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace App.YandexClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYandexClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationKey)
    {
        services.Configure<YandexClientConfiguration>(configuration.GetSection(configurationKey));
        return services
            .AddScoped(SpeechKitClientFactory)
            .AddScoped<IYandexClient, YandexClient>();
    }

    private static SpeechKitClient SpeechKitClientFactory(IServiceProvider sp)
    {
        var yandexClientConfigurationOptionsSnapshot = sp.GetRequiredService<IOptionsSnapshot<YandexClientConfiguration>>();
        var yandexClientConfiguration = yandexClientConfigurationOptionsSnapshot.Value;
        var speechKitClientOptions = new SpeechKitClientOptions(
            yandexClientConfiguration.ApiKey,
            yandexClientConfiguration.ApplicationName,
            yandexClientConfiguration.UserId,
            yandexClientConfiguration.Device);

        return new SpeechKitClient(speechKitClientOptions);
    }
}
