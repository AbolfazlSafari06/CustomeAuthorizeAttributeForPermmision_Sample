using Microsoft.EntityFrameworkCore;
using WebApi.Database;
using WebApi.DTOs;
using WebApi.Hash;
using WebApi.Models;

namespace WebApi.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _context;

    public UserRepository(DataBaseContext context)
    {
        _context = context;
    }

    public List<User> GetList()
    {
        return _context.Users.ToList();
    }

    public User Get(int id)
    {
        return _context.Users.FirstOrDefault(x => x.Id.Equals(id));
    }

    public bool Delete(int id)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id.Equals(id));
        if (user is null)
        {
            return false;
        }
        _context.Users.Remove(user);
        return true;
    }
     

    public string[] GetUserPermissions(int userId)
    {
        var isUserExist = _context.Users.Any(x=>x.Id.Equals(userId));
        if (isUserExist == false)
        {
            throw new Exception("User Not Found");
        }

        var userPermissions = _context
            .UserPermissions
            .Include(x=>x.User)
            .Include(x=>x.Permission)
            .AsNoTracking()
            .Where(x => x.UserId.Equals(userId))
            .Select(x => x.Permission.Name)
            .ToList();

        return userPermissions.ToArray();
    }

    public User GetByUserName(string userName)
    {
        var isUserExist = _context.Users.Any(x => x.Name.Equals(userName));
        if (isUserExist == false)
        {
            throw new Exception("User Not Found");
        }

        return _context.Users.FirstOrDefault(x => x.Name.Equals(userName));
    }

    public bool CheckUserHasPermission(int userId, string[] requiredPermissions)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id.Equals(userId));
        
        if (user is null)
        {
            throw new Exception("User Not Found");
        }

        //var userPermission = _context
        //    .UserPermissions
        //    .Include(x => x.User)
        //    .Include(x => x.Permission)
        //    .AsNoTracking()
        //    .Where(x => x.UserId.Equals(userId))
        //    .Select(x=>new {x.UserId , x.Permission.Name})
        //    .ToList();


        //var result1 = _context
        //    .UserPermissions
        //    .Include(x => x.User)
        //    .Include(x => x.Permission)
        //    .AsNoTracking()
        //    .Where(x => x.UserId.Equals(userId))
        //    .Any(x => x.Permission.Name.Equals("strign1"));

        //var requiredPermissionsZero = requiredPermissions[0];
        //var result2 = requiredPermissions[0].Equals("strign1");
        //var result3 = requiredPermissionsZero.Equals("strign1");

        var result = _context
            .UserPermissions
            .Include(x => x.User)
            .Include(x => x.Permission)
            .AsNoTracking()
            .Where(x => x.UserId.Equals(userId))
            .Any(x => x.Permission.Name.Equals(requiredPermissions[0]));

        return result;
    }

    public void Create(CreateUser command)
    {
        var hashPassword = PasswordHasher.Hash(command.Password);
        var listCount = _context.Users.Count();
        
        _context.Users.Add(new User()
        {
            Password = hashPassword,
            Name = command.Name,
        });
    }

    public bool Update(UpdateUser command)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id.Equals(command.Id));
        if (user == null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(command.Name) == false)
        {
            user.Name = command.Name;
        }
        if (string.IsNullOrEmpty(command.Password) == false)
        {
            var hashPassword = PasswordHasher.Hash(command.Password);
            user.Name = hashPassword;
        }
        //SetUserPermission(command);
        return true;

    }

    public async Task<bool> CheckUserHasPermissionAsync(int userId, string[] requiredPermissions)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id.Equals(userId));

        if (user is null)
        {
            throw new Exception("User Not Found");
        }

        var userPermission = _context
            .UserPermissions
            .Include(x => x.User)
            .Include(x => x.Permission)
            .AsNoTracking()
            .Where(x => x.UserId.Equals(userId))
            .Select(x => new { x.UserId, x.Permission.Name });


        var result = _context
            .UserPermissions
            .Include(x => x.User)
            .Include(x => x.Permission)
            .AsNoTracking()
            .Where(x => x.UserId.Equals(userId))
            .Any(x => x.Permission.Name.Equals(requiredPermissions[0]));

        return result;
    }
}
