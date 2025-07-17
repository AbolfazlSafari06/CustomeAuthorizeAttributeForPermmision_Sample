using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repository;
using System.Threading.Tasks;

namespace WebApi.Attributes;
 
public class CustomAuthorizeAttribute : TypeFilterAttribute
{
    public CustomAuthorizeAttribute(params string[] permissions)
        : base(typeof(CustomAuthorizeFilter)) // Specifies which filter to use
    {
        // Passes arguments to the filter
        Arguments = new object[] { permissions }; 
    }
}

public class CustomAuthorizeFilter : IAuthorizationFilter
{
    private string[] _requiredPermissions;
    public readonly IUserRepository _userRepository;

    // Constructor receives parameters from the attribute
    public CustomAuthorizeFilter(string[] permissions, IUserRepository userRepository)
    {
        _requiredPermissions = permissions;
        _userRepository = userRepository;
    }
    //public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //{
    //    // Actual authorization logic here
    //    if (!context.HttpContext.User.Identity.IsAuthenticated)
    //    {
    //        context.Result = new UnauthorizedResult();
    //        return;
    //    }

    //    if (_requiredPermissions.Length == 0)
    //    {
    //        return;
    //    }

    //    // Check permissions against user claims
    //    var userIdInString = context.HttpContext
    //        .User
    //        .Claims
    //        .Where(x => x.Type.Equals("USERID"))
    //        .First();

    //    var userId = int.Parse(userIdInString.Value);
    //    bool hasPermission = await _userRepository.CheckUserHasPermissionAsync(userId, _requiredPermissions);

    //    if (!hasPermission)
    //    {
    //        context.Result = new ForbidResult();
    //    }
    //}

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Actual authorization logic here
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (_requiredPermissions.Length == 0)
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
        bool hasPermission = _userRepository.CheckUserHasPermission(userId, _requiredPermissions);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}