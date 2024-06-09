using CarrotParser.Application.Persistence;

namespace CarrotParser.Application.Database
{
    internal interface IDbManager
    {
        IPersonsRepository Repository { get; }
        void MoveDatabase(string oldPath, string newPath);
    }
}