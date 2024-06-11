using CarrotParser.Application.Persistence;
using LiteDB;

namespace CarrotParser.Application.Database;

internal class DbManager : IDisposable, IDbManager
{
    private string _path = string.Empty;
    private LiteDatabase? _database;
    private IPersonsRepository? _repository;

    public IPersonsRepository CreateDatabase(string value)
    {
        _path = value;
        _database = new LiteDatabase(_path);
        _repository = new PersonsRepository(_database);
        return _repository;
    }

    public IPersonsRepository? GetRepository()
    {
        try
        {
            if (_database is null)
            {
                _database = new LiteDatabase(_path);
                _repository = new PersonsRepository(_database);
            }
            return _repository;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void MoveDatabase(string oldPath, string newPath)
    {
        if (_database is null)
        {
            throw new NullReferenceException($"Can't find database in {oldPath}");
        }
        _database.Dispose();
        File.Move(oldPath, newPath);
        _path = newPath;
        _database = new(_path);
        _repository = new PersonsRepository(_database);
    }

    public void Dispose()
    {
        _database?.Dispose();
    }
}
