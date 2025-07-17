namespace WebApi.DTOs;

public class CreateUser
{
    public string Name { get; set; }
    public string Password { get; set; }
    public List<string> Permissions { get; set; }
}
