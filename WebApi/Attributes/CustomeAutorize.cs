using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database;
using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Attributes;
 
public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute(params string[] permissions)
        : base(typeof(CustomAuthorizeFilter)) // Specifies which filter to use
    {
        Arguments = new object[] { permissions }; // Passes arguments to the filter
    }
}

public class CustomAuthorizeFilter : IAuthorizationFilter
{
    private string[] _requiredPermissions;
    public readonly DataBase _database;

    // Constructor receives parameters from the attribute
    public CustomAuthorizeFilter(string[] permissions, DataBase database)
    {
        _requiredPermissions = permissions;
        _database = database;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Actual authorization logic here
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if(_requiredPermissions.Length == 0)
        {
            return;
        }
        // Check permissions against user claims
        var userIdInString = context.HttpContext
            .User
            .Claims
            .Where(x => x.Type.Equals("USERID"))
            .First();

        var userId = int.Parse(userIdInString.Value);
        bool hasPermission = _database.CheckUserHasPermission(userId, _requiredPermissions);
         
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}