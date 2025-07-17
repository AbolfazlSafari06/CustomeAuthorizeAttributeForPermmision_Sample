using WebApi.Hash;
using WebApi.Models;

namespace WebApi.Database;

public class DataBase
{
    private List<User> users;
    public DataBase()
    {
        users = new List<User>();
        users.Add(new User
        {
            Id = 1,
            Name = "admin",
            Password = "123",
            //Permissions = ["string1", "string2"]
        });
    }

    public List<User> GetList()
    {
        return users;
    }

    public User Get(int id)
    {
        return users.FirstOrDefault(x => x.Id.Equals(id));
    }

    public void Create(User newUser)
    {
        var hashPassword = PasswordHasher.Hash(newUser.Password);
        var listCount = users.Count();
        users.Add(new User()
        {
            Password = hashPassword,
            Permissions = newUser.Permissions,
            Name = newUser.Name,
            Id = listCount + 1
        });
    }

    public bool Update(int id, User newUser)
    {
        var user = users.FirstOrDefault(x => x.Id.Equals(id));
        if (user == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(newUser.Name) == false)
        {
            user.Name = newUser.Name;
        }
        if (string.IsNullOrEmpty(newUser.Password) == false)
        {
            var hashPassword = PasswordHasher.Hash(newUser.Password);
            user.Name = hashPassword;
        }
        user.Permissions = newUser.Permissions;
        return true;
    }

    public bool Delete(int id)
    {
        var user = users.FirstOrDefault(x => x.Id.Equals(id));
        if (user is null)
        {
            return false;
        }
        users.Remove(user);
        return true;
    }

    public bool SetUserPermission(int userId, string[] permission)
    {
        var user = users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            return false;
        }
        //user.Permissions = permission.ToList();
        return true;
    }

    public string[] GetUserPermissions(int userId)
    {
        var user = users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            throw new Exception("User Not Found");
        }
        return new string[] { user.Name };
        //return user.Permissions.ToArray();
    }

    public User GetByUserName(string userName)
    {
        return users.FirstOrDefault(x => x.Name.Equals(userName));
    }

    public bool CheckUserHasPermission(int userId, string[] requiredPermissions)
    {
        var user = users.FirstOrDefault(x => x.Id.Equals(userId));
        if (user is null)
        {
            throw new Exception("User Not Found");
        }
        var result = false;
        foreach (var permission in requiredPermissions)
        {
            if(user.Permissions.Any(x => x.Equals(permission)))
            {
                result = true;
                break;
            }
        }
        return result;
    }
}
