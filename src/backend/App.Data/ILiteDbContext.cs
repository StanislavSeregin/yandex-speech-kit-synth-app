using LiteDB;
using System;

namespace App.Data;

public interface ILiteDbContext : IDisposable
{
    ILiteCollection<T> GetCollection<T>();

    ILiteStorage<Guid> GetStorage();
}
