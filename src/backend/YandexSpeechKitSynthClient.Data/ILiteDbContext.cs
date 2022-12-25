using LiteDB;
using System;

namespace YandexSpeechKitSynthClient.Data;

public interface ILiteDbContext : IDisposable
{
    ILiteCollection<T> GetCollection<T>();

    ILiteStorage<Guid> GetStorage();
}
