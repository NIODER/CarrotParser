using CarrotParser.Application.Model;
using LiteDB;

namespace CarrotParser.Application.Persistence;

internal class PersonsRepository(LiteDatabase db) : IPersonsRepository
{
    private readonly LiteDatabase _db = db;
    private ILiteCollection<Person> Persons => _db.GetCollection<Person>();

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
