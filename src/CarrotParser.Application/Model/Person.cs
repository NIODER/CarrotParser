using LiteDB;

namespace CarrotParser.Application.Model;

public class Person
{
    public ObjectId Id { get; set; } = null!;
    public string Gender { get; set; } = null!;
    public Name Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Login Login { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Nat { get; set; } = null!;
}
