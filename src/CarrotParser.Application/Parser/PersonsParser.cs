using CarrotParser.Application.Model;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CarrotParser.Application.Parser;

internal class PersonsParser : IPersonsParser
{
    private const string URL = @"https://randomuser.me/api?inc=gender,name,login,email,nat,phone";
    private readonly static JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<Person> GetPersonAsync(CancellationToken cancellationToken)
    {
        var people = await GetPersonValuesAsync<Person>(cancellationToken);
        return people.First();
    }

    public Task<List<Person>> GetPersonsCollectionAsync(int count, CancellationToken cancellationToken)
    {
        return GetPersonValuesAsync<Person>(cancellationToken, count);
    }

    public async IAsyncEnumerable<Person> GetPersonsAsync(int count, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (int i = 0; i < count && !cancellationToken.IsCancellationRequested; i++)
        {
            yield return await GetPersonAsync(cancellationToken);
        }
    }

    private static async Task<List<T>> GetPersonValuesAsync<T>(CancellationToken cancellationToken, int count = 1)
    {
        var json = await GetJsonAsync(count, cancellationToken);
        return JsonNode.Parse(json)?["results"]?.Deserialize<List<T>>(_jsonOptions)  // из-за структуры ответа всегда там массив
            ?? throw new NullReferenceException("Can't parse person's json.");
    }

    private static async Task<string> GetJsonAsync(int count, CancellationToken cancellationToken)
    {
        using var httpClient = new HttpClient();
        string response = await httpClient.GetStringAsync(URL + $"&results={count}", cancellationToken);
        string? errorMessage = JsonNode.Parse(response)?["error"]?.GetValue<string>();
        if (errorMessage is not null)
        {
            throw new Exception(errorMessage);
        }
        return response;
    }
}
