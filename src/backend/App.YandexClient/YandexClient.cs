using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.YandexClient;

internal class YandexClient : IYandexClient
{
    private readonly HttpClient _httpClient;

    public YandexClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Stream> TextToSpeechInRussian(
        string text,
        CancellationToken cancellationToken)
    {
        var requestDict = new Dictionary<string, string>
        {
            { "text", text },
            { "lang", "ru-RU" },
            { "voice", "alena" },
            { "emotion", "neutral" },
            { "speed", "1.0" },
            { "format", "oggopus" },
            { "sampleRateHertz", "48000" }
        };

        var requestContent = new FormUrlEncodedContent(requestDict);
        var response = await _httpClient.PostAsync(
            "/speech/v1/tts:synthesize",
            requestContent,
            cancellationToken);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStreamAsync(cancellationToken);
    }
}
