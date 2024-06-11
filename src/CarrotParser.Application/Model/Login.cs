namespace CarrotParser.Application.Model;

public class Login
{
    public Guid Uuid { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Md5 { get; set; } = null!;
    public string Sha1 { get; set; } = null!;
    public string Sha256 { get; set; } = null!;
}
