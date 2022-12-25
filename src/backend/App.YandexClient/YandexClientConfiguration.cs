using System;

namespace App.YandexClient;

public class YandexClientConfiguration
{
    public string ApiKey { get; } = string.Empty;

    public string ApplicationName { get; } = string.Empty;

    public string Device { get; } = string.Empty;

    public Guid UserId { get; } = Guid.NewGuid();
}
