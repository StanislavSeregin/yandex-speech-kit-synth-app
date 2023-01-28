using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.YandexClient;

internal class YandexClient : IYandexClient
{
    private readonly HttpClient _httpClient;
    private readonly YandexClientConfiguration _yandexClientConfiguration;

    public YandexClient(
        HttpClient httpClient,
        IOptionsSnapshot<YandexClientConfiguration> yandexClientConfigurationOptionsSnapshot)
    {
        _httpClient = httpClient;
        _yandexClientConfiguration = yandexClientConfigurationOptionsSnapshot.Value;
    }

    public async Task<Stream> TextToSpeechInRussian(
        string text,
        CancellationToken cancellationToken)
    {
        var textToSpeechRequest = new TextToSpeechRequest(
            text,
            null,
            "ru-RU",
            "alena",
            "neutral",
            "1.0",
            "oggopus",
            "48000",
            null);

        var response = await _httpClient.PostAsJsonAsync(
            "/speech/v1/tts:synthesize",
            textToSpeechRequest,
            cancellationToken);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}
