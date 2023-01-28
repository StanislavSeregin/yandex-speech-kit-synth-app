using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace App.YandexClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYandexClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string configurationKey)
    {
        var settingsSection = configuration.GetSection(configurationKey);
        services.Configure<YandexClientConfiguration>(settingsSection);
        services.AddHttpClient<IYandexClient, YandexClient>(httpClient =>
        {
            var yandexClientConfiguration = new YandexClientConfiguration();
            settingsSection.Bind(yandexClientConfiguration);
            httpClient.BaseAddress = new Uri(yandexClientConfiguration.YandexUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Api-Key",
                yandexClientConfiguration.ApiKey);
        });

        return services;
    }
}
