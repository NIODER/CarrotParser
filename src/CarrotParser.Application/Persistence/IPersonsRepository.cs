using CarrotParser.Application.Model;
using LiteDB;
using System.Linq.Expressions;

namespace CarrotParser.Application.Persistence;

public interface IPersonsRepository
{
    void CreatePerson(Person person);
    int DeleteAll();
    void DeletePerson(ObjectId id);
    List<Person> Get(int shift, int count);
    List<Person> GetBetweenDateTimes(DateTime dateTimeSince, DateTime dateTimeTo);
    List<Person> GetPersonsByEmail(string email);
    List<Person> GetPersonByUsername(string username);
    Person? GetPersonById(ObjectId id);
    void UpdatePerson(Person person);
}