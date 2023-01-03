using ITCC.YandexSpeechKitClient;
using ITCC.YandexSpeechKitClient.Enums;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace App.YandexClient;

internal class YandexClient : IYandexClient
{
    private readonly SpeechKitClient _speechKitClient;

    public YandexClient(SpeechKitClient speechKitClient)
    {
        _speechKitClient = speechKitClient;
    }

    public async Task<Stream> TextToSpeechInRussian(
        string text,
        CancellationToken cancellationToken)
    {
        var options = new SynthesisOptions(text)
        {
            AudioFormat = SynthesisAudioFormat.Wav,
            Language = SynthesisLanguage.Russian,
            Emotion = Emotion.Neutral,
            Quality = SynthesisQuality.High,
            Speaker = Speaker.Alyss
        };

        var textToSpechResult = await _speechKitClient.TextToSpeechAsync(
            options,
            cancellationToken);

        var isFailed = textToSpechResult is
        { TransportStatus: not TransportStatus.Ok } or
        { ResponseCode: not HttpStatusCode.OK };

        if (isFailed)
        {
            throw new Exception(
                $"Yandex response error: {nameof(TransportStatus)} is {textToSpechResult.TransportStatus}, {nameof(ResponseCode)} is {textToSpechResult.TransportStatus}");
        }

        return textToSpechResult.Result;
    }
}
