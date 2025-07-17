namespace WebApi.Models;

public class User
{
    public User()
    {

    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public List<UserPermission> Permissions { get; set; }
}
