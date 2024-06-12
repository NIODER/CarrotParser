using CarrotParser.Application.Model;
using LiteDB;
using System.Linq.Expressions;

namespace CarrotParser.Application.Persistence;

internal class PersonsRepository(LiteDatabase database) : IPersonsRepository
{
    private ILiteCollection<Person> Persons => database.GetCollection<Person>();

    public void CreatePerson(Person person)
    {
        Persons.Insert(person);
    }

    public Person? GetPersonById(ObjectId id)
    {
        return Persons.Query()
            .Where(p => p.Id == id)
            .FirstOrDefault();
    }

    public List<Person> GetPersonsByEmail(string email)
    {
        return Persons.Query()
            .Where(p => p.Email == email)
            .ToList();
    }

    public List<Person> GetPersonByUsername(string username)
    {
        return Persons.Query()
            .Where(p => p.Login.Username == username)
            .ToList();
    }

    public void UpdatePerson(Person person)
    {
        Persons.Update(person);
    }

    public void DeletePerson(ObjectId id)
    {
        Persons.Delete(id);
    }

    public int DeleteAll()
    {
        return Persons.DeleteAll();
    }

    public List<Person> Get(int shift, int count)
    {
        return Persons.Query()
            .Offset(shift)
            .Limit(count)
            .ToList();
    }

    public List<Person> GetBetweenDateTimes(DateTime dateTimeSince, DateTime dateTimeTo)
    {
        return Persons.Query()
            .Where(p => p.Id.CreationTime.Date >= dateTimeSince && p.Id.CreationTime.Date <= dateTimeTo)
            .ToList();
    }
}
