using CarrotParser.Application.Persistence;

namespace CarrotParser.Application.Database;

public interface IDbManager
{
    IPersonsRepository CreateDatabase(string value);
    IPersonsRepository? GetRepository();
    void MoveDatabase(string oldPath, string newPath);
}