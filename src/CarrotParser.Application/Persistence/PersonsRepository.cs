using CarrotParser.Application.Database;
using CarrotParser.Application.Model;
using LiteDB;

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

    public Person? GetPersonByEmail(string email)
    {
        return Persons.Query()
            .Where(p => p.Email == email)
            .FirstOrDefault();
    }

    public Person GetPersonByUsername(string username)
    {
        return Persons.Query()
            .Where(p => p.Login.Username == username)
            .FirstOrDefault();
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
}
