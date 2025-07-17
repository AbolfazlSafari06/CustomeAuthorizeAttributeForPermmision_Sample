namespace WebApi.Models;

public class Permission
{
    public Permission()
    {

    }
    public int Id { get; set; }
    public string Name { get; set; }
    public List<UserPermission> Users { get; set; }
}
