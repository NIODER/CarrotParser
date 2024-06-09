using CarrotParser.Application.Model;
using LiteDB;

namespace CarrotParser.Application.Persistence;

public interface IPersonsRepository
{
    void CreatePerson(Person person);
    int DeleteAll();
    void DeletePerson(ObjectId id);
    Person? GetPersonByEmail(string email);
    Person? GetPersonById(ObjectId id);
    Person GetPersonByUsername(string username);
    void UpdatePerson(Person person);
}