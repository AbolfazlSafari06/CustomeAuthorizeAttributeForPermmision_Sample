namespace WebApi.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<string> Permissions { get; set; }
    public string Password { get; set; }
}
