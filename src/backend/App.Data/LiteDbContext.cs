using LiteDB;
using System;
using System.IO;
using System.Reflection;

namespace App.Data;

internal class LiteDbContext : ILiteDbContext, IDisposable
{
    private readonly ILiteDatabase _liteDatabase;

    public LiteDbContext()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var currentLocation = Path.GetDirectoryName(executingAssemblyPath);
        var dbPath = Path.Combine(currentLocation, "data.db");
        _liteDatabase = new LiteDatabase(dbPath);
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
