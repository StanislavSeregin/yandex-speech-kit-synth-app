using LiteDB;
using System;

namespace App.Data;

internal class LiteDbContext : ILiteDbContext, IDisposable
{
    private readonly ILiteDatabase _liteDatabase;

    public LiteDbContext()
    {
        _liteDatabase = new LiteDatabase("./data.db");
    }

    public ILiteCollection<T> GetCollection<T>()
        => _liteDatabase.GetCollection<T>();

    public ILiteStorage<Guid> GetStorage()
        => _liteDatabase.GetStorage<Guid>();

    public void Dispose()
    {
        _liteDatabase.Dispose();
        GC.SuppressFinalize(this);
    }
}
