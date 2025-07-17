using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository;

public interface IUserRepository
{
    List<User> GetList();
    User Get(int id);
    void Create(CreateUser command);
    bool Update(UpdateUser commmand);
    bool Delete(int id);
    string[] GetUserPermissions(int userId);
    User GetByUserName(string userName);
    bool CheckUserHasPermission(int userId, string[] requiredPermissions);
    Task<bool> CheckUserHasPermissionAsync(int userId, string[] requiredPermissions);
}
