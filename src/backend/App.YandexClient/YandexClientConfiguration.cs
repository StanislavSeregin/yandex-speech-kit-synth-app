using System;

namespace App.YandexClient;

public class YandexClientConfiguration
{
    public string ApiKey { get; set; } = string.Empty;

    public string ApplicationName { get; set; } = "desktop-app-tts";

    public string Device { get; set; } = $"desktop-{Environment.OSVersion.Platform}".ToLower();

    public Guid UserId { get; set; } = Guid.NewGuid();
}
