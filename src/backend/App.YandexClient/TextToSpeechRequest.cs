namespace App.YandexClient;

internal record TextToSpeechRequest(
    string Text,
    string Smsl,
    string Lang,
    string Voice,
    string Emotion,
    string Speed,
    string Format,
    string SampleRateHertz,
    string FolderId);
