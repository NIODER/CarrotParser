using LiteDB;

namespace CarrotParser.Application.Model;

public class Person
{
    public ObjectId Id { get; set; }
    public string Gender { get; set; }
    public Name Name { get; set; }
    public string Email { get; set; }
    public Login Login { get; set; }
    public string Phone { get; set; }
    public string Nat { get; set; }
}
