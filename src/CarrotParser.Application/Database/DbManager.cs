using CarrotParser.Application.Persistence;
using LiteDB;

namespace CarrotParser.Application.Database;

internal class DbManager : IDbManager, IDisposable
{
    private LiteDatabase _database = null!;
    private IPersonsRepository _repository = null!;

    public DbManager(string path)
    {
        _database = new(path);
        _repository = new PersonsRepository(_database);
    }

    public IPersonsRepository Repository => _repository;

    public void MoveDatabase(string oldPath, string newPath)
    {
        _database.Dispose();
        File.Move(oldPath, newPath);
        _database = new(newPath);
        _repository = new PersonsRepository(_database);
    }

    public void Dispose()
    {
        _database.Dispose();
    }
}
