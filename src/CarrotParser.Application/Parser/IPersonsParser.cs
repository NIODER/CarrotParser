using CarrotParser.Application.Model;

namespace CarrotParser.Application.Parser;

public interface IPersonsParser
{
    Task<Person> GetPersonAsync(CancellationToken cancellationToken);
    IAsyncEnumerable<Person> GetPersonsAsync(int count, CancellationToken cancellationToken);
    Task<List<Person>> GetPersonsCollectionAsync(int count, CancellationToken cancellationToken);
}