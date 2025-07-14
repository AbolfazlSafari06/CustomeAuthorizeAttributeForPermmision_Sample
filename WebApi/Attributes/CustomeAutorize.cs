using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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

    // Constructor receives parameters from the attribute
    public CustomAuthorizeFilter(string[] permissions)
    {
        _requiredPermissions = permissions;
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
        var userPermissions = context.HttpContext
            .User.
            Claims
            .Where(x=>x.Type.Equals("permission"))
            .Select(x=>x.Value)
            .ToList();

        var userName = context.HttpContext.User.Identity.Name;

        var hasPermission = false;

        // Check permissions against user claims 
        foreach (var per in _requiredPermissions)
        {
            if (userPermissions.Any(x => x.Equals(per)))
            {
                hasPermission = true;
                break;
            }
        }

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}